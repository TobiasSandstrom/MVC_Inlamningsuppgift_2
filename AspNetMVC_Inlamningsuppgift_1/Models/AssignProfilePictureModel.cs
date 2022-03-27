using System.ComponentModel.DataAnnotations;

namespace AspNetMVC_Inlamningsuppgift_2.Models
{
    public class AssignProfilePictureModel
    {

        [Display(Name ="Picture Upload")]
        [Required(ErrorMessage = "You must choose a picture if you want to upload an image")]
        public IFormFile FormFile { get; set; }

        public string ErrorMessage { get; set; } = "";
        public string SuccessMessage { get; set; } = "";

        
    }
}
