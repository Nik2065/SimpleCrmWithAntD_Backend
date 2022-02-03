using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    [Table("CrmAccounts")]
    public class CrmAccount
    {
        [Key]
        public Guid CrmAccountId { get; set; }
        //Наименование организации
        public string CompanyName { get; set; }
    }
}
