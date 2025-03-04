using BusinessObject.Repository;
using DataAccessLayer.Models;

namespace BusinessObject.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<NewsArticle> _newsRepository;

        public CategoryService(IRepository<Category> categoryRepository, IRepository<NewsArticle> newsRepository)
        {
            _categoryRepository = categoryRepository;
            _newsRepository = newsRepository;
        }

        // Kiểm tra xem Category có đang được dùng trong bài viết nào không
        public bool CanDeleteCategory(short categoryId)
        {
            // Nếu có bất kỳ bài viết nào có CategoryId trùng => không xóa được
            return !_newsRepository.GetAll().Any(n => n.CategoryId.HasValue && n.CategoryId.Value == categoryId);
        }

        public Category GetById(short categoryId)
        {
            return _categoryRepository.GetAll().FirstOrDefault(c => c.CategoryId == categoryId);
        }

        public IEnumerable<Category> GetAll()
        {
            return _categoryRepository.GetAll();
        }

        public void CreateCategory(Category category)
        {
            _categoryRepository.Insert(category);
            _categoryRepository.Save();
        }

        public void UpdateCategory(Category category)
        {
            _categoryRepository.Update(category);
            _categoryRepository.Save();
        }

        public void DeleteCategory(short categoryId)
        {
            var category = GetById(categoryId);
            if (category != null)
            {
                _categoryRepository.Delete(category);
                _categoryRepository.Save();
            }
        }
    }

}
