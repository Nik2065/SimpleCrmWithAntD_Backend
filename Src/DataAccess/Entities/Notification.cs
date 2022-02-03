using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    [Table("Notifications")]
    public class Notification
    {
        [Key]
        public Guid Id { get; set; }
        //Наименование организации
        public string Body { get; set; }
        public string Subject { get; set; }
        public Guid CrmUserId { get; set; }
        public DateTime? Sended { get; set; }

    }
}
