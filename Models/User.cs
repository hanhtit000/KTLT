using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace StaffManagement.Models
{
    public class User
    {
        public User() {
            Schedules = new HashSet<Schedule>();
        }
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string StaffNumber { get; set; }

        [Required(ErrorMessage = "Please enter Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter PhoneNumber")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please enter Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter DOB")]
        public DateOnly DOB { get; set; }

        [Required(ErrorMessage = "Please enter Address")]
        public string Address { get; set; }
        [ForeignKey(nameof(Department))]
        public int? DepartmentId { get; set; }
        public string Description { get; set; }

        public virtual Account Account { get; set; } = null!;
        public virtual Department Department { get; set; } = null!;

        public virtual ICollection<Schedule> Schedules { get; set; } = null!;
    }
}
