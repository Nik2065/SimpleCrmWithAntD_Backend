using DataAccess;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using SimpleCrmApi.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCrmApi.Services
{
   

    //public class UserService : IUserService
    //{
    //    //
    //    // TODO: кеширование таблицы пользователей
    //    //

    //    public UserService(DataStore context)
    //    {
    //        _db = context;
    //        _users = _db.CrmUsers.Include(u => u.UserRole).ToList();
    //        //_users = _db.Users.ToList();
    //    }

    //    private DataStore _db;
    //    private List<CrmUser> _users;

    //    public async Task<CrmUser> Authenticate(string useremail, string password)
    //    {
    //        var b64 = HashPassword(password);
    //        var user = await Task.Run(() => _users
    //            .SingleOrDefault(x => x.CrmUserEmail == useremail && x.CrmUserPassword == b64));

    //        // return null if user not found
    //        if (user == null)
    //            return null;

    //        // authentication successful so return user details without password
    //        return user.WithoutPassword();
    //    }

    //    public string HashPassword(string pwd)
    //    {
    //        SHA512 shaM = new SHA512Managed();
    //        var hashed = shaM.ComputeHash(Encoding.UTF8.GetBytes(pwd));
    //        var b64 = Convert.ToBase64String(hashed);
    //        return b64;
    //    }

        //public async Task<IEnumerable<PortalUser>> GetAll()
        //{
        //    return await Task.Run(() => _users.WithoutPasswords());
        //}
    //}
}
