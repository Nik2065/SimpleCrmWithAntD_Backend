using Common;
using DataAccess;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Logic
{

    public interface IUserLogic
    {
        Task<CrmUser> Authenticate(string username, string password);
        //Task<IEnumerable<PortalUser>> GetAll();
        string HashPassword(string password);
    }

    public class UserLogic : IUserLogic
    {
      


        public UserLogic(DataStore dataStore)
        {
            _db = dataStore;
        }


        private DataStore _db;
        private List<CrmUser> _users;

        public async Task<CrmUser> Authenticate(string useremail, string password)
        {
            var b64 = HashPassword(password);
            var user = await Task.Run(() => _users
                .SingleOrDefault(x => x.CrmUserEmail == useremail && x.CrmUserPassword == b64));

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so return user details without password
            return user.WithoutPassword();
        }

        public string HashPassword(string pwd)
        {
            SHA512 shaM = new SHA512Managed();
            var hashed = shaM.ComputeHash(Encoding.UTF8.GetBytes(pwd));
            var b64 = Convert.ToBase64String(hashed);
            return b64;
        }

        //
        //
        //

        public async Task RegUser(RegData data)
        {


            using (var tran = new TransactionScope())
            {
                var account = new CrmAccount();
                account.CompanyName = data.CompanyName;

                _db.CrmAccounts.Add(account);
                await _db.SaveChangesAsync();

                var user = new CrmUser();
                user.CrmAccountId = account.CrmAccountId;
                user.CrmUserEmail = data.UserEmail;
                user.CrmUserPassword = this.HashPassword(data.Password);
                user.FirstName = data.FirstName;
                user.SecondName = data.SecondName;
                user.ThirdName = data.ThirdName;
                user.CrmUserPhone = data.UserPhone;

                _db.CrmUsers.Add(user);
                await _db.SaveChangesAsync();

                tran.Complete();
            }
        }

        public void CreateTestUser()
        {

            //var us = new UserService(_db);

            var a = new CrmAccount();
            a.CompanyName = "Test Company";
            _db.CrmAccounts.Add(a);
            _db.SaveChanges();



            var u = new CrmUser();
            u.CrmUserEmail = "mail@projectsrv.ru";
            u.CrmUserPassword = HashPassword("123");
            u.CrmUserPhone = "79001234455";
            u.FirstName = "";
            u.CrmUserRoleId = (int)CrmUserRolesEnum.Administrator;
            u.CrmAccountId = a.CrmAccountId;
            u.CrmUserCreated = DateTime.Now;


            _db.CrmUsers.Add(u);
            _db.SaveChanges();

        }

    }
}
