using System.ComponentModel.DataAnnotations;

namespace AspNetMVC_Inlamningsuppgift_2.Models
{
    public class SignInModel
    {

        [Display(Name = "Email")]
        [Required(ErrorMessage = "You need to enter a {0}")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "{0} is not valid")]
        public string Email { get; set; } = "";


        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "You must enter a {0}")]
        public string Password { get; set; } = "";

        public string ErrorMessage { get; set; } = "";
        public string ReturnUrl { get; set; } = "/";

    }
}
