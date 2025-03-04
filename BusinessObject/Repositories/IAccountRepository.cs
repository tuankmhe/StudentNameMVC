using BusinessObject.Repository;
using DataAccessLayer.Models;

namespace BusinessObject.Repositories
{
    public interface IAccountRepository : IRepository<SystemAccount>
    {
        SystemAccount GetAccountByEmail(string email);
        SystemAccount GetAccountById(short id);
    }

}
