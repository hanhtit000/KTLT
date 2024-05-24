using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StaffManagement.Models
{
    public class UserActivity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string CreateById { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Description { get; set; }

    }
}
