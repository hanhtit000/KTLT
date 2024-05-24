using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StaffManagement.Models
{
    public class Department
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Please enter Code")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Please enter Name")]
        public string Name { get; set; }
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public DateTime CreatedOn { get; set; }
        public virtual User User { get; set; }
    }
}
