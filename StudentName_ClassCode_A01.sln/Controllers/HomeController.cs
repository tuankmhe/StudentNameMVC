using BusinessObject.Services;
using Microsoft.AspNetCore.Mvc;
using StudentName_ClassCode_A01.sln.Models;
using System.Diagnostics;

namespace StudentName_ClassCode_A01.sln.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INewsService _newsService;
        public HomeController(ILogger<HomeController> logger, INewsService newsService)
        {
            _logger = logger;
            _newsService = newsService;
        }

        public IActionResult Index()
        {
            // Kiểm tra xem người dùng đã đăng nhập chưa
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail")))
            {
                return RedirectToAction("Login", "Account");
            }

            // Lấy role từ session và chuyển sang tên (nếu cần hiển thị)
            int? role = HttpContext.Session.GetInt32("UserRole");
            string roleName = role switch
            {
                1 => "Staff",
                2 => "Lecturer",
                3 => "Admin",
                _ => "Unknown"
            };
            ViewBag.Role = roleName;

            // Lấy danh sách bài viết active (ví dụ)
            var activeNews = _newsService.GetAllNews()
                .Where(n => n.NewsStatus.HasValue && n.NewsStatus.Value);
            return View(activeNews);
        }
        public IActionResult AccessDenied()
        {
            return View();
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
