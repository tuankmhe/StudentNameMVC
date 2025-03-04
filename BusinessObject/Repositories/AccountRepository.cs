using BusinessObject.Repository;
using DataAccessLayer.Models;

namespace BusinessObject.Repositories
{
    public class AccountRepository : Repository<SystemAccount>, IAccountRepository
    {
        public AccountRepository(FunewsManagementContext context) : base(context) { }

        public SystemAccount GetAccountByEmail(string email)
        {
            return _context.SystemAccounts.FirstOrDefault(a => a.AccountEmail == email);
        }
        public SystemAccount GetAccountById(short id)
        {
            return _context.SystemAccounts.FirstOrDefault(a => a.AccountId == id);
        }
    }
}
