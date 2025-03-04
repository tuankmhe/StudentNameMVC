using DataAccessLayer.Models;

namespace BusinessObject.Services
{
    public interface ICategoryService
    {
        bool CanDeleteCategory(short categoryId);
        Category GetById(short categoryId);
        IEnumerable<Category> GetAll();
        void CreateCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(short categoryId);
    }
}
