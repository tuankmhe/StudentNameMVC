using BusinessObject.Services;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentName_ClassCode_A01.Models;

namespace StudentName_ClassCode_A01.Controllers
{
    //  [Authorize(Roles = "Admin")]//
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public IActionResult Index(string searchString)
        {
            // Kiểm tra đăng nhập
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail")))
            {
                return RedirectToAction("Login", "Account");
            }
            // Kiểm tra quyền Admin
            int? role = HttpContext.Session.GetInt32("UserRole");
            if (!role.HasValue || role.Value != 3)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            var accounts = string.IsNullOrEmpty(searchString)
                ? _accountService.GetAllAccounts()
                : _accountService.SearchAccounts(searchString);

            return View(accounts);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var account = _accountService.Login(model.Email, model.Password);
                if (account != null)
                {
                    // Lưu thông tin đăng nhập vào Session
                    HttpContext.Session.SetString("UserEmail", account.AccountEmail);
                    HttpContext.Session.SetInt32("UserRole", (int)account.AccountRole); // ví dụ: 1:Staff, 2:Lecturer, 3:Admin
                    HttpContext.Session.SetInt32("UserId", account.AccountId);

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Thông tin đăng nhập không hợp lệ.");
            }
            return View(model);
        }

        // Logout: Xóa Session và chuyển về trang Login
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
        // GET: Tạo tài khoản (Trang riêng, không dùng modal)
        public IActionResult Create()
        {
            // Kiểm tra quyền Admin
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail")) ||
                HttpContext.Session.GetInt32("UserRole") != 3)
            {
                return RedirectToAction("AccessDenied", "Account");
            }
            return View();
        }

        // POST: Tạo tài khoản
        [HttpPost]
        public IActionResult Create(SystemAccount account)
        {
            // Kiểm tra quyền Admin
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail")) ||
                HttpContext.Session.GetInt32("UserRole") != 3)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            if (ModelState.IsValid)
            {
                _accountService.CreateAccount(account);
                return RedirectToAction("Index");
            }
            return View(account);
        }

        // GET: Sửa tài khoản (Trang riêng)
        public IActionResult Edit(short id)
        {
            // Kiểm tra quyền Admin
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail")) ||
                HttpContext.Session.GetInt32("UserRole") != 3)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            var acc = _accountService.GetAccountById(id);
            if (acc == null)
                return NotFound();
            return View(acc);
        }

        // POST: Sửa tài khoản
        [HttpPost]
        public IActionResult Edit(SystemAccount account)
        {
            // Kiểm tra quyền Admin
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail")) ||
                HttpContext.Session.GetInt32("UserRole") != 3)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            if (ModelState.IsValid)
            {
                _accountService.UpdateAccount(account);
                return RedirectToAction("Index");
            }
            return View(account);
        }

        // Xóa tài khoản
        public IActionResult Delete(short id)
        {
            // Kiểm tra quyền Admin
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail")) ||
                HttpContext.Session.GetInt32("UserRole") != 3)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            // Thường có trang xác nhận xóa, hoặc JavaScript confirm
            _accountService.DeleteAccount(id);
            return RedirectToAction("Index");
        }
    }
}

