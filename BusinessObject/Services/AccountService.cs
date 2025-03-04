using BusinessObject.Repositories;
using DataAccessLayer.Models;

namespace BusinessObject.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public SystemAccount Login(string email, string password)
        {
            var account = _accountRepository.GetAccountByEmail(email);
            if (account != null && account.AccountPassword == password)
            {
                return account;
            }
            return null;
        }

        public IEnumerable<SystemAccount> GetAllAccounts() => _accountRepository.GetAll();
        public SystemAccount GetAccountById(short id) => _accountRepository.GetAccountById(id);
        public void CreateAccount(SystemAccount acc)
        {
            _accountRepository.Insert(acc);
            _accountRepository.Save();
        }
        public void UpdateAccount(SystemAccount acc)
        {
            _accountRepository.Update(acc);
            _accountRepository.Save();
        }
        public void DeleteAccount(short id)
        {
            // Kiểm tra logic, ví dụ không cho xóa chính mình
            _accountRepository.Delete(GetAccountById(id));
            _accountRepository.Save();
        }
        public IEnumerable<SystemAccount> SearchAccounts(string keyword)
        {
            return null;
        }
    }
}
