using QLNhaHang.API.Entities;
using QLNhaHang.API.Helpers;

namespace QLNhaHang.API.Interfaces
{
    public interface IAccountService
    {
        public PageResult<Account> Get(Pagination? pagination = null);


        public Account Insert(Account user);


        public Account Update(string userId, Account user);

        public Account Login(Account user);
    }
}
