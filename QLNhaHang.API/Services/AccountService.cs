using QLNhaHang.API.Entities;
using QLNhaHang.API.Exceptions;
using QLNhaHang.API.Interfaces;
using QLNhaHang.API.Utils;

namespace QLNhaHang.API.Services
{
    public class AccountService : IAccountService
    {
        private readonly QLNhaHangContext dbContext;
        public AccountService()
        {
            dbContext = new QLNhaHangContext();
        }
        public IEnumerable<Account> Get()
        {
            return dbContext.Accounts.ToList();
        }

        public Account Insert(Account user)
        {
            using (var trans = dbContext.Database.BeginTransaction())
            {
                EntityUtils<Account>.ValidateData(user);
                //Check duplicate username
                var userCheck = dbContext.Accounts.FirstOrDefault(x => x.UserName == user.UserName);
                if(userCheck != null)
                {
                    throw new QLNhaHangException(Resource.QLNhaHangResource.UserNameExist);
                }
                else
                {
                    user.UserId = Guid.NewGuid().ToString();
                    user.Status = Enums.Status.Active;
                    user.CreatedTime = DateTime.Now;
                    dbContext.Accounts.Add(user);
                    dbContext.SaveChanges();
                    trans.Commit();
                    return user;
                }
            }
        }

        public Account Login(Account user)
        {
            if(user.UserName == null || user.Password == null)
            {
                throw new QLNhaHangException(Resource.QLNhaHangResource.NotEmptyLogin);
            }
            var userFind = dbContext.Accounts.FirstOrDefault(x => x.UserName == user.UserName && x.Password == user.Password);
            if(userFind == null)
            {
                throw new QLNhaHangException(Resource.QLNhaHangResource.AccountNotFound);
            }
            else
            {
                if(userFind.Status == Enums.Status.Disable)
                {
                    throw new QLNhaHangException(Resource.QLNhaHangResource.AccountDisable);
                }
                return userFind;
            }
        }

        public Account Update(string userId, Account user)
        {
            using (var trans = dbContext.Database.BeginTransaction())
            {
                EntityUtils<Account>.ValidateData(user);
                var userFind = dbContext.Accounts.Find(userId);
                if(userFind == null)
                {
                    throw new QLNhaHangException(Resource.QLNhaHangResource.AccountNotFound);
                }
                else
                {
                    userFind.Password = user.Password;
                    userFind.FullName = user.FullName;
                    userFind.PhoneNumber = user.PhoneNumber;
                    userFind.Email = user.Email;
                    userFind.Position = user.Position;
                    userFind.Status = user.Status;
                    userFind.UpdatedTime = DateTime.Now;
                    dbContext.Update(userFind);
                    dbContext.SaveChanges();
                    trans.Commit();
                    return userFind;
                }
            }
        }
    }
}
