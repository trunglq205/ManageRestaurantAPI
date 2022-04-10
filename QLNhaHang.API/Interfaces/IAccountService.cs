using QLNhaHang.API.Entities;

namespace QLNhaHang.API.Interfaces
{
    public interface IAccountService
    {
        public IEnumerable<Account> Get();


        public Account Insert(Account user);


        public Account Update(string userId, Account user);

        public Account Login(Account user);
    }
}
