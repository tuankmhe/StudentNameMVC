using BusinessObject.Services;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace StudentName_ClassCode_A01.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IAccountService _accountService;

        public ProfileController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // Xem hồ sơ cá nhân
        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail")))
            {
                return RedirectToAction("Login", "Account");
            }

            int? userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
                return RedirectToAction("Login", "Account");

            var account = _accountService.GetAccountById((short)userId.Value);
            if (account == null)
                return NotFound();

            return View(account);
        }

        // GET: Chỉnh sửa hồ sơ cá nhân
        public IActionResult Edit()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail")))
            {
                return RedirectToAction("Login", "Account");
            }

            int? userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
                return RedirectToAction("Login", "Account");

            var account = _accountService.GetAccountById((short)userId.Value);
            if (account == null)
                return NotFound();

            return View(account);
        }

        // POST: Chỉnh sửa hồ sơ cá nhân
        [HttpPost]
        public IActionResult Edit(SystemAccount account)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail")))
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                _accountService.UpdateAccount(account);
                return RedirectToAction("Index");
            }
            return View(account);
        }
    }
}
