using System.ComponentModel.DataAnnotations;

namespace AspNetMVC_Inlamningsuppgift_1.Models
{
    public class MeetingModel
    {

        [Display(Name ="Name")]
        [Required(ErrorMessage = "You need to enter a name")]
        [StringLength(256, ErrorMessage = "Name must be atleast {2} characters long", MinimumLength = 2)]
        public string? CustomerName { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "You must enter a email")]
        [EmailAddress(ErrorMessage = "You must enter a valid email")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Invalid email format.")]
        public string? CustomerEmail { get; set; }
        
        [Display(Name = "Message")]
        [Required(ErrorMessage = "You need to enter a message")]
        [StringLength(500, ErrorMessage = "Message must be atleast {2} characters long", MinimumLength = 12)]
        public string? CustomerMessage { get; set; }

    }
}
