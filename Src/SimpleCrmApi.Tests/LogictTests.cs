using NUnit.Framework;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using DataAccess.Entities;
using Logic;

namespace SimpleCrmApi.Tests
{
    public class LogictTests
    {
        public LogictTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder<DataStore>()
            .UseInMemoryDatabase("InMemoryDatabase")
            .Options;

            _db = new DataStore(dbContextOptions);

            //prepearing data

            // Roles
            //
            var r1 = new CrmUserRole { CrmUserRoleId = 1, Code = "Operator" };
            var r2 = new CrmUserRole { CrmUserRoleId = 2, Code = "Manager" };
            var r3 = new CrmUserRole { CrmUserRoleId = 3, Code = "Administrator" };
            _db.CrmUsersRoles.AddRange(r1, r2, r3);
            _db.SaveChanges();

            // TestUser
            //


        }


        [SetUp]
        public void Setup()
        {


        }

        private readonly DataStore _db;
        //
        private const string TestUserEmail = "mail@projectsrv.ru";




        [Test]
        public void CanCreateTestUser()
        {
            var userLogic = new UserLogic(_db);
            userLogic.CreateTestUser();

            //check that user in test database
            var dbuser = _db.CrmUsers.FirstOrDefaultAsync(x => x.CrmUserEmail == TestUserEmail);

            if (dbuser == null)
                Assert.Fail("Cannot find test user");

            Assert.Pass();
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}