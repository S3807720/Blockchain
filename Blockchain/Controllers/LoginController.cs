using Blockchain.Data;
using Blockchain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleHashing;

namespace Blockchain.Controllers
{

    [AllowAnonymous]
    [Route("/Blockchain/SecureLogin")]
    public class LoginController : Controller
    {
        private readonly BlockchainContext _context;

        public LoginController(BlockchainContext context) => _context = context;

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(int loginID, string password)
        {
            var login = await _context.Logins.FindAsync(loginID);
            if (login == null)
            {
                ModelState.AddModelError("LoginFailed", "Login failed, please try again.");
                return View(new Login { LoginID = loginID });
            }
            var user = await _context.Users.FindAsync(login.UserID);
            if(!PBKDF2.Verify(login.PasswordHash, password))
            {
                ModelState.AddModelError("LoginFailed", "Login failed, please try again.");
                return View(new Login { LoginID = loginID });
            }
            // Login customer.
            HttpContext.Session.SetInt32(nameof(BCUser.UserID), login.UserID);
            HttpContext.Session.SetString(nameof(BCUser.Name), login.User.Name);
            HttpContext.Session.SetInt32(nameof(login.LoginID), login.LoginID);
            return RedirectToAction("Index", "Home");
        }

        [Route("LogoutNow")]
        public IActionResult Logout()
        {
            // Logout customer.
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }
        
       
    }
}
