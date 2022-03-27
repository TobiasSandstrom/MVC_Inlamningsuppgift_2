
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AspNetMVC_Inlamningsuppgift_1.Models
{

    public class AdminEditViewModel 
    { 



        [Key]
        public string Id { get; set; } = string.Empty;

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "You need to enter a {0}")]
        [StringLength(300, ErrorMessage = "{0} must be atleast {2} characters", MinimumLength = 2)]
        public string FirstName { get; set; } = string.Empty;


        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "You need to enter a {0}")]
        [StringLength(300, ErrorMessage = "{0} must be atleast {2} characters", MinimumLength = 2)]
        public string LastName { get; set; } = string.Empty;


        public string FullName => $"{FirstName} {LastName}";


        [Display(Name = "Email")]
        [Required(ErrorMessage = "You need to enter a {0}")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "{0} is not valid")]
        public string Email { get; set; } = string.Empty;


        [Display(Name = "Street Address")]
        [Required(ErrorMessage = "You must enter a {0}")]
        [StringLength(256, ErrorMessage = "{0} must be atleast {2} characters", MinimumLength = 2)]
        [RegularExpression(@"^([a-öA-Ö]+?)([\s][0-9]+)*?$", ErrorMessage = "Must be a valid streetname")]
        public string Street { get; set; } = string.Empty;


        [Display(Name = "Zipcode")]
        [Required(ErrorMessage = "You need to enter a {0}")]
        [StringLength(5, ErrorMessage = "{0} must be 5 characters", MinimumLength = 5)]
        public string Zipcode { get; set; } = string.Empty;


        [Display(Name = "City")]
        [Required(ErrorMessage = "You need to enter a {0}")]
        [StringLength(300, ErrorMessage = "{0} must be atleast {2} characters", MinimumLength = 2)]
        public string City { get; set; } = string.Empty;


        [Display(Name = "Country")]
        [Required(ErrorMessage = "You need to enter a {0}")]
        [StringLength(300, ErrorMessage = "{0} must be atleast {2} characters", MinimumLength = 2)]
        public string Country { get; set; } = string.Empty;


        //[Display(Name = "Role")]
        //[Required(ErrorMessage = "You need to enter a {0}")]
        //[StringLength(100, ErrorMessage = "{0} must be atleast {2} characters", MinimumLength = 2)]
        //public string PageRole { get; set; } = string.Empty;

        public string Response { get; set; } = "";

        //public List<IdentityRole>? AllRoles { get; set; }

    }

}
