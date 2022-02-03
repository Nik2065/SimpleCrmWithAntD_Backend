using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCrmApi.Helpers
{
    public static class ExtensionMethods
    {
        public static IEnumerable<CrmUser> WithoutPasswords(this IEnumerable<CrmUser> users)
        {
            return users.Select(x => x.WithoutPassword());
        }

        public static CrmUser WithoutPassword(this CrmUser user)
        {
            user.CrmUserPassword = null;
            return user;
        }
    }
}
