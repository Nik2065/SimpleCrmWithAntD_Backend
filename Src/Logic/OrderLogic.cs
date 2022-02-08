using DataAccess;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logic
{
    public class OrderLogic
    {

        public OrderLogic(DataStore dataStore)
        {
            _db = dataStore;
        }


        private readonly DataStore _db;

        public List<CrmOrder> GetCrmOrders(CrmOrdersCondition condition)
        {
            //var result = new List<CrmOrder>();

            var result = _db.CrmOrders
                       .Where(item => item.CrmAccountId == condition.CrmAccountId)
                       .Include(item => item.CrmCustomer)
                       .Include(item => item.AuthorCrmUser)
                       .ToList();


            return result;
        }

    }

    public class CrmOrdersCondition
    {
        public Guid CrmAccountId { get; set; }

    }
}
