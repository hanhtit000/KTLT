using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StaffManagement.Models
{
    public class Account
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter Password")]
        public string Password { get; set; }
        public bool IsAdmin { get; set; }

        public bool IsActive { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;
    }
}
