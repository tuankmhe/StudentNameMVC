using DataAccessLayer.Models;

namespace BusinessObject.Services
{
    public interface IAccountService
    {
        SystemAccount Login(string email, string password);
        IEnumerable<SystemAccount> GetAllAccounts();
        SystemAccount GetAccountById(short id);
        void CreateAccount(SystemAccount acc);
        void UpdateAccount(SystemAccount acc);
        void DeleteAccount(short id);
        IEnumerable<SystemAccount> SearchAccounts(string keyword);
    }
}
