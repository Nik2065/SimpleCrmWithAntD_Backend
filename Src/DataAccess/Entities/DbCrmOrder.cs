using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataAccess.Entities
{
    [Table("CrmOrders")]
    public class DbCrmOrder
    {
        
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public string Content { get; set; }

        public Guid AuthorId { get; set; }
        public Guid AccountId { get; set; }
    }
}
