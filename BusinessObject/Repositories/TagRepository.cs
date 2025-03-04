using BusinessObject.Repository;
using DataAccessLayer.Models;

namespace BusinessObject.Repositories
{
    public class TagRepository : Repository<Tag>, ITagRepository
    {
        public TagRepository(FunewsManagementContext context) : base(context)
        {
        }
    }
}
