using BusinessObject.Repository;
using DataAccessLayer.Models;

namespace BusinessObject.Repositories
{
    public class CategoryRepository(FunewsManagementContext context) : Repository<Category>(context), ICategoryRepository
    {
    }
}
