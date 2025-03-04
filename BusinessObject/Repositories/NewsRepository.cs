using BusinessObject.Repository;
using DataAccessLayer.Models;

namespace BusinessObject.Repositories
{
    public class NewsRepository : Repository<NewsArticle>, INewsRepository
    {
        public NewsRepository(FunewsManagementContext context) : base(context)
        {
        }
    }
}
