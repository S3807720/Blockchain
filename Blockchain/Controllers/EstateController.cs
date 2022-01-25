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
    public class EstateController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BlockchainContext _context;


        public EstateController(ILogger<HomeController> logger, BlockchainContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            BlockChainUtility.Load();
            var blocks = BlockChainUtility._chain.Chain.ToList();
            List<Property> properties = new();
            List<BCApplication> apps = new();
            List<Property> props = new();
            foreach (var b in blocks)
            {
                apps.AddRange(b.Transactions);
            }
            apps.AddRange(BlockChainUtility._chain.pendingTransactions);
            var f = apps.OfType<PermitDecision>().ToList();
            foreach (var p in apps.OfType<PermitApplication>().ToList())
            {
                props.Add(p.Property);
            }
            foreach (var app in f)
            {
                if (app.Approved == true) { properties.AddRange(props.Where(x => x.Address == app.Address).ToList()); }
            }
            foreach (var r in apps.OfType<PendingTransaction>().ToList())
            {
                properties.Remove(properties.Where(x => x.PropertyID == r.PropertyID).FirstOrDefault());
            }
            return View(properties);
        }

        public IActionResult Offer(int PropertyID, int BuyerID)
        {
            BlockChainUtility.Load();
            _context.PendingTransactions.Add(new PendingTransaction { BuyerID = BuyerID, PropertyID = PropertyID });
            _context.SaveChanges();
            ViewBag.Offer = $"Offer for {PropertyID} successfully sent.";
            BlockChainUtility.Use
            (_context.PendingTransactions.OrderByDescending(x => x.BCApplicationID).FirstOrDefault());
            return RedirectToAction("Index");
        }

        public IActionResult ShowOffers(int property, int buyer)
        {
            BlockChainUtility.Load();
            List<OfferViewModel> vm = new();
            var blocks = BlockChainUtility._chain.Chain.ToList();
            List<Property> properties = new();
            List<BCApplication> apps = new();
            List<PendingTransaction> offers = new();
            foreach (var b in blocks)
            {
                apps.AddRange(b.Transactions);
            }
            apps.AddRange(BlockChainUtility._chain.pendingTransactions);
            foreach (var p in apps.OfType<PermitApplication>().ToList())
            {
                properties.Add(p.Property);
            }
            var f = apps.OfType<PendingTransaction>().ToList();
            foreach (var app in f)
            {
                foreach (var decision in apps.OfType<TransactionDecision>().ToList())
                {
                    if (app.BCApplicationID == decision.PendingTransactionsID)
                    {
                        app.Accepted = decision.Accepted;
                    }
                }
                if (app.Accepted == null) { offers.Add(app); }
            }

            foreach(var off in offers)
            {
                string addr = properties.Where(x => x.PropertyID == off.PropertyID).FirstOrDefault().Address;
                var LoanID = apps.OfType<LoanApplication>().Where(x => x.Buyer.UserID == off.BuyerID && 
                    x.Address == addr).FirstOrDefault().BCApplicationID;
                vm.Add(new OfferViewModel { Buyers = _context.Buyers.Where(x => x.UserID == off.BuyerID).FirstOrDefault().Name,
                    Address = addr,
                    Loan = apps.OfType<LoanDecision>().Where(x => x.LoanID == LoanID).FirstOrDefault(),
                    PendingTransactionID = off.BCApplicationID
                });
            }
            return View(vm);
        }

        public IActionResult AcceptOffer(int BCApplicationID)
        {
            ConfirmOffer(BCApplicationID, true);
            return RedirectToAction("ShowOffers");
        }

        public IActionResult RejectOffer(int BCApplicationID)
        {
            ConfirmOffer(BCApplicationID, false);
            return RedirectToAction("ShowOffers");
        }

        public void ConfirmOffer(int perm, bool approve)
        {
            var permit = _context.Permits.Find(perm);
            var decision = new TransactionDecision { Accepted = approve, PendingTransactionsID = perm };
            _context.TransactionDecisions.Add(decision);
            _context.SaveChanges();
            BlockChainUtility.Use
                (_context.TransactionDecisions.OrderByDescending(x => x.BCApplicationID).FirstOrDefault());
            BlockChainUtility.Mine();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}