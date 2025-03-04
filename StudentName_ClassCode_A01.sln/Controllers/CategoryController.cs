using BusinessObject.Services;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace StudentName_ClassCode_A01.Controllers
{
    //[Authorize(Roles = "Staff")]  // Giả sử Staff được phép quản lý chuyên mục
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // Hiển thị danh sách chuyên mục, có thể có chức năng tìm kiếm
        public IActionResult Index()
        {
            // Kiểm tra đăng nhập và quyền Staff
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail")))
                return RedirectToAction("Login", "Account");
            int? role = HttpContext.Session.GetInt32("UserRole");
            if (!role.HasValue || role.Value == 1)
                return RedirectToAction("AccessDenied", "Account");

            var categories =
                 _categoryService.GetAll();

            return View(categories);
        }

        // GET: Hiển thị form tạo chuyên mục
        public IActionResult Create()
        {
            return View();
        }

        // POST: Xử lý tạo chuyên mục mới
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category model)
        {
            if (ModelState.IsValid)
            {
                _categoryService.CreateCategory(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Hiển thị form chỉnh sửa chuyên mục
        public IActionResult Edit(short id)
        {
            var category = _categoryService.GetById(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Xử lý cập nhật chuyên mục
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category model)
        {
            if (ModelState.IsValid)
            {
                _categoryService.UpdateCategory(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Hiển thị trang xác nhận xóa chuyên mục
        public IActionResult Delete(short id)
        {
            var category = _categoryService.GetById(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Xóa chuyên mục đã xác nhận
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(short id)
        {
            // Kiểm tra xem chuyên mục có thể xóa không (không được sử dụng bởi bài viết nào)
            if (!_categoryService.CanDeleteCategory(id))
            {
                TempData["Error"] = "Không thể xóa chuyên mục vì nó đang được sử dụng bởi bài viết.";
                return RedirectToAction("Index");
            }
            _categoryService.DeleteCategory(id);
            return RedirectToAction("Index");
        }
    }
}
