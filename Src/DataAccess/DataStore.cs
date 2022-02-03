using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class DataStore : DbContext
    {
        public DataStore(DbContextOptions<DataStore> options)
            : base(options)
        {


        }

        public DbSet<CrmUser> CrmUsers { get; set; }
        public DbSet<CrmUserRole> CrmUsersRoles { get; set; }
        public DbSet<CrmOrder> CrmOrders { get; set; }
        public DbSet<CrmAccount> CrmAccounts { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<CrmCustomer> CrmCustomers { get; set; }

        public async Task<int> GenerateLocatOrderId()
        {
            var max = await this.CrmOrders.MaxAsync(item => item.LocalId);
            return max + 1;
        }

        public async Task<int> GenerateLocatCustomerId()
        {
            var max = await this.CrmCustomers.MaxAsync(item => item.LocalId);
            return max + 1;
        }
    }
}
