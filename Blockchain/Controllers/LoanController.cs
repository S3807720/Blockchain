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
    public class LoanController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BlockchainContext _context;


        public LoanController(ILogger<HomeController> logger, BlockchainContext context)
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
        public IActionResult Index(string Address, decimal LoanAmount)
        {
            Console.WriteLine("ADdress & Loan amnt: " + Address + LoanAmount);
            //  if (property == null) { ModelState.AddModelError("Submit", "Fields cannot be empty."); return View(property); }
            //  if (!ModelState.IsValid) { return View(property); }
            // if (files == null || files.Length == 0) { ModelState.AddModelError("BuildingDesign", "You must upload a building design."); return View(property); }
            /*  var user = _context.Buyers.Where(X => X.UserID == HttpContext.Session.GetInt32(nameof(BCUser.UserID))).FirstOrDefault();
              if(user == null)
              {
                  return View();
              }*/
            var user = _context.Buyers.FirstOrDefault();
            _context.Loans.Add(new LoanApplication { Address = Address, LoanAmount = LoanAmount, Buyer = user });
            _context.SaveChanges();
            BlockChainUtility.Use
                (_context.Loans.OrderByDescending(x => x.BCApplicationID).FirstOrDefault());
            ViewBag.LoanMessage = "Loan application successfully received.";
            ModelState.Clear();
            return View();
        }
        public IActionResult ShowLoans()
        {
            BlockChainUtility.Load();
            var blocks = BlockChainUtility._chain.Chain.ToList();
            List<BCApplication> apps = new();
            List<LoanApplication> loans = new();
            foreach (var b in blocks)
            {
                apps.AddRange(b.Transactions);
            }
            apps.AddRange(BlockChainUtility._chain.pendingTransactions);
            var f = apps.OfType<LoanApplication>().ToList();
            foreach (var loanApp in f)
            {
                foreach (var decision in apps.OfType<LoanDecision>().ToList())
                {
                    if (loanApp.BCApplicationID == decision.LoanID)
                    {
                        loanApp.Decision = decision.Approved;
                    }

                }
                loans.Add(loanApp);
            }
            return View(loans);
        }

        public IActionResult ApproveLoan(int BCApplicationID, int BuyerID)
        {
            ConfirmLoan(BCApplicationID, true, BuyerID);
            return RedirectToAction("ShowLoans");
        }

        public IActionResult RejectLoan(int BCApplicationID, int BuyerID)
        {
            ConfirmLoan(BCApplicationID, false, BuyerID);
            return RedirectToAction("ShowLoans");
        }

        public void ConfirmLoan(int perm, bool approve, int buyerid)
        {
            var permit = _context.Loans.Find(perm);
            var buyer = _context.Buyers.Find(buyerid);
            var decision = new LoanDecision { Approved = approve, LoanID = permit.BCApplicationID, Buyer = buyer };
            _context.LoanDecisions.Add(decision);
            _context.SaveChanges();
            BlockChainUtility.Use
                (_context.LoanDecisions.OrderByDescending(x => x.BCApplicationID).FirstOrDefault());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}