using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    [Table("CrmCustomers")]
    public class CrmCustomer
    {
        [Key]
        public Guid CustomerId { get; set; }
        //индектификатор в пределах аккаунта
        public int LocalId { get; set; }

        public string CustomerFirstName { get; set; }
        public string CustomerSecondName { get; set; }
        public string CustomerThirdName { get; set; }

        public string CustomerPhone { get; set; }

        public Guid CrmAccountId { get; set; }
        public Guid AuthorCrmUserId { get; set; }

        //public Guid CustomerId { get; set; }
        public string CustomerEmail { get; set; }

        public DateTime Created { get; set; }
    }
}
