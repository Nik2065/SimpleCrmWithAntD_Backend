using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    [Table("CrmOrders")]
    public class CrmOrder
    {
        [Key]
        //глобальный идентификатор
        public Guid Id { get; set; }
        //порядковый номер в пределах аккаунта
        public int LocalId { get; set; }
        public string Description { get; set; }

        public Guid CrmAccountId { get; set; }
        
        [ForeignKey("AuthorCrmUser")]
        public Guid AuthorCrmUserId { get; set; }
        public CrmUser AuthorCrmUser { get; set; }
        public Guid ResponsibleCrmUserId { get; set; }

        [ForeignKey("CrmCustomer")]
        public Guid CustomerId { get; set; }
        public CrmCustomer CrmCustomer { get; set; }

        public DateTime CrmOrderCreated { get; set; }
    }
}
