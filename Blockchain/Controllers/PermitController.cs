using Blockchain.Data;
using Blockchain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Diagnostics;
using System.IO;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Formatters.Binary;

namespace Blockchain.Controllers
{
    public class PermitController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BlockchainContext _context;


        public PermitController(ILogger<HomeController> logger, BlockchainContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            
            return View();
        }

        [HttpPost]
        [DisableRequestSizeLimit,
        RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue,
        ValueLengthLimit = int.MaxValue)]
        public IActionResult Index(Property property, IFormFile files)
        {
          //  if (property == null) { ModelState.AddModelError("Submit", "Fields cannot be empty."); return View(property); }
          //  if (!ModelState.IsValid) { return View(property); }
           // if (files == null || files.Length == 0) { ModelState.AddModelError("BuildingDesign", "You must upload a building design."); return View(property); }

            //Getting FileName
            var fileName = Path.GetFileName(files.FileName);
            //Getting file Extension
            var fileExtension = Path.GetExtension(fileName);
            // concatenating  FileName + FileExtension
            var newFileName = String.Concat(Convert.ToString(Guid.NewGuid()), fileExtension);

            var objfiles = new BCFile()
            {
                Name = newFileName,
            };

            using (var target = new MemoryStream())
            {
                files.CopyTo(target);
                objfiles.Content = target.ToArray();
            }
            property.BuildingDesign = objfiles;
            var permit = new PermitApplication { Property = property };
            _context.Properties.Add(property);
            _context.Permits.Add(permit);
            _context.Files.Add(objfiles);
            _context.SaveChanges();
            BlockChainUtility.Use
                (_context.Permits.OrderByDescending(x => x.BCApplicationID).FirstOrDefault());
            ViewBag.SuccessMessage = "Permit successfully received.";
            ModelState.Clear();
            return View();
        }
    
        public IActionResult ShowPermits()
        {
            BlockChainUtility.Load();
            var blocks = BlockChainUtility._chain.Chain.ToList();
         
            List<BCApplication> apps = new();
            List<PermitApplication> permits = new();
            foreach(var b in blocks)
            {
                apps.AddRange(b.Transactions);
            }
            apps.AddRange(BlockChainUtility._chain.pendingTransactions);
            var f = apps.OfType<PermitApplication>().ToList();
            foreach (var permitApp in f)
            {
                foreach (var decision in apps.OfType<PermitDecision>().ToList())
                {
                    if (permitApp.BCApplicationID == decision.PermitID)
                    {
                        permitApp.Decision = decision.Approved;
                    }

                }
                permits.Add(permitApp);
            }
            return View(permits);
        }
        public IActionResult ApprovePermit(int BCApplicationID, int PropertyID)
        {
            ConfirmPermit(BCApplicationID, true, PropertyID);
            return RedirectToAction("ShowPermits");
        }

        public IActionResult RejectPermit(int BCApplicationID, int PropertyID)
        {
            ConfirmPermit(BCApplicationID, false, PropertyID);
            return RedirectToAction("ShowPermits");
        }

        public void ConfirmPermit(int perm, bool approve, int prop)
        {
            var permit = _context.Permits.Find(perm);
            var property = _context.Properties.Find(prop);
            var decision = new PermitDecision { Approved = approve, PermitID = permit.BCApplicationID, Address = property.Address };
            _context.PermitDecisions.Add(decision);
            _context.SaveChanges();
            BlockChainUtility.Use
                (_context.PermitDecisions.OrderByDescending(x => x.BCApplicationID).FirstOrDefault());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}