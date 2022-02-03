using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataAccess.Entities
{
    [Table("CrmUsers")]
    public class CrmUser
    {
        [Key]
        public Guid CrmUserId { get; set; }
        //Имя
        public string FirstName { get; set; }
        //Фамилия
        public string SecondName { get; set; }
        //Отчество
        public string ThirdName { get; set; }

        //Используется как логин
        public string CrmUserEmail { get; set; }
        public string CrmUserPhone { get; set; }
        public string CrmUserPassword { get; set; }

        [ForeignKey("UserRole")]
        public int CrmUserRoleId { get; set; }
        public CrmUserRole UserRole { get; set; }


        [ForeignKey("UserRoleOf")]
        public Guid CrmAccountId { get; set; }
        public CrmAccount CrmAccountOf { get; set; }

        public DateTime CrmUserCreated { get; set; }
    }
}
