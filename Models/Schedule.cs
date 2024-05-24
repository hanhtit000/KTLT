using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StaffManagement.Models
{
    public class Schedule
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter Name")]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required(ErrorMessage = "Please enter Date")]
        public DateOnly Date { get; set; }

        [Required(ErrorMessage = "Please enter StartTime")]
        public TimeOnly StartTime { get; set; }

        [Required(ErrorMessage = "Please enter EndTime")]
        public TimeOnly EndTime {  get; set; }

        [Required(ErrorMessage = "Please select Staff")]
        public bool IsAccept {  get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;
    }
}
