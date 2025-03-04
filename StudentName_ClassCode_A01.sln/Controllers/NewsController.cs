using BusinessObject.Services;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace StudentName_ClassCode_A01.Controllers
{
    //[Authorize(Roles = "Staff")]  // Giả sử Staff có quyền tạo và chỉnh sửa tin tức
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;
        private readonly ICategoryService _categoryService;

        public NewsController(INewsService newsService, ICategoryService categoryService)
        {
            _newsService = newsService;
            _categoryService = categoryService;
        }

        // Hiển thị danh sách bài viết của Staff
        public IActionResult Index(string searchString)
        {
            // Kiểm tra đăng nhập
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail")))
                return RedirectToAction("Login", "Account");

            // Kiểm tra quyền: chỉ Staff (UserRole = 1) mới có quyền quản lý tin tức của mình
            int? role = HttpContext.Session.GetInt32("UserRole");
            if (!role.HasValue)
                return RedirectToAction("AccessDenied", "Account");

            // Lấy UserId từ Session
            int? staffId = HttpContext.Session.GetInt32("UserId");
            IEnumerable<NewsArticle> newsList;
            if (string.IsNullOrEmpty(searchString))
                newsList = _newsService.GetNewsByStaff((short)staffId.Value);
            else
                newsList = _newsService.SearchNewsByStaff((short)staffId.Value, searchString);

            return View(newsList);
        }

        // Hiển thị chi tiết bài viết
        public IActionResult Details(string id)
        {
            var news = _newsService.GetAllNews().FirstOrDefault(n => n.NewsArticleId == id);
            if (news == null)
            {
                return NotFound();
            }
            return View(news);
        }

        // GET: Hiển thị form tạo bài viết
        public IActionResult Create()
        {
            // Lấy danh sách chuyên mục để chọn
            ViewBag.Categories = new SelectList(_categoryService.GetAll(), "CategoryId", "CategoryName");
            return View();
        }

        // POST: Xử lý tạo bài viết mới
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(NewsArticle model, string? tagNames)
        {
            if (ModelState.IsValid)
            {
                // Chuyển chuỗi tag (danh sách tag cách nhau bởi dấu phẩy) thành danh sách string
                var tags = tagNames.Split(',')
                                   .Select(t => t.Trim())
                                   .Where(t => !string.IsNullOrEmpty(t))
                                   .ToList();

                // Gán các giá trị bổ sung
                model.CreatedDate = DateTime.Now;
                model.NewsStatus = true;
                model.CreatedById = GetStaffId();

                _newsService.CreateNews(model, tags);
                return RedirectToAction("Index");
            }
            ViewBag.Categories = new SelectList(_categoryService.GetAll(), "CategoryId", "CategoryName");
            return View(model);
        }

        // GET: Hiển thị form chỉnh sửa bài viết
        public IActionResult Edit(string id)
        {
            var news = _newsService.GetAllNews().FirstOrDefault(n => n.NewsArticleId == id);
            if (news == null)
            {
                return NotFound();
            }
            ViewBag.Categories = new SelectList(_categoryService.GetAll(), "CategoryId", "CategoryName", news.CategoryId);
            // Chuẩn bị danh sách tag dạng chuỗi (ví dụ: "Tag1,Tag2,Tag3")
            ViewBag.TagNames = string.Join(",", news.Tags.Select(t => t.TagName));
            return View(news);
        }

        // POST: Xử lý cập nhật bài viết
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(NewsArticle model, string? tagNames)
        {
            if (ModelState.IsValid)
            {
                var tags = tagNames.Split(',')
                                   .Select(t => t.Trim())
                                   .Where(t => !string.IsNullOrEmpty(t))
                                   .ToList();

                _newsService.UpdateNews(model, tags);
                return RedirectToAction("Index");
            }
            ViewBag.Categories = new SelectList(_categoryService.GetAll(), "CategoryId", "CategoryName", model.CategoryId);
            return View(model);
        }

        // GET: Xóa bài viết (hiển thị trang xác nhận)
        public IActionResult Delete(string id)
        {
            var news = _newsService.GetAllNews().FirstOrDefault(n => n.NewsArticleId == id);
            if (news == null)
            {
                return NotFound();
            }
            return View(news);
        }

        // POST: Xóa bài viết đã xác nhận
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            _newsService.DeleteNews(id);
            return RedirectToAction("Index");
        }

        private short GetStaffId()
        {
            int? staffId = HttpContext.Session.GetInt32("UserId");
            if (staffId.HasValue)
            {
                return (short)staffId.Value;
            }
            else
            {
                // Xử lý khi chưa đăng nhập hoặc không tìm thấy thông tin trong Session
                throw new Exception("User is not logged in.");
            }
        }

    }
}
