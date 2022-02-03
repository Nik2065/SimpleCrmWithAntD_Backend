using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    [Table("CrmUserRoles")]
    public class CrmUserRole
    {
        [Key]
        public int CrmUserRoleId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
