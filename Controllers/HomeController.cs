using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Authenticator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TwoFactorAuth.Models;

namespace TwoFactorAuth.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var userEmail = "test@gmail.com"; // you can get this from user context or other sources
            var twoFactorAuthenticator = new TwoFactorAuthenticator();
            var secretCode = Guid.NewGuid().ToString().Replace("-", "")[0..10];
            var accountSecretKey = $"{secretCode}-{userEmail}";
            var setupCode = twoFactorAuthenticator.GenerateSetupCode("2FA", userEmail,
                Encoding.ASCII.GetBytes(accountSecretKey));

            HomeViewModel vm = new HomeViewModel
            {
                Email = userEmail,
                QrCodeUrl = setupCode.QrCodeSetupImageUrl,
                AuthCode = setupCode.ManualEntryKey,
                SecretKey = accountSecretKey
            };

            ViewBag.HomeModel = vm;

            return View(vm);
        }

        [HttpPost]
        public IActionResult Validate(string SecretCode, string UserCode)
        {

            var twoFactorAuthenticator = new TwoFactorAuthenticator();
            var result = twoFactorAuthenticator.ValidateTwoFactorPIN(SecretCode, UserCode);

            if (result)
                return RedirectToAction("Privacy");
            else
                return RedirectToAction("Error");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
