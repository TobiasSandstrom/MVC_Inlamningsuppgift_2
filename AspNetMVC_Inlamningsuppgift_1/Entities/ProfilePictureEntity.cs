using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetMVC_Inlamningsuppgift_2.Entities
{
    public class ProfilePictureEntity
    {
        [Key]
        public string Picture { get; set; }

        [Required]
        [Column(TypeName ="nvarchar(450)")]
        public string UserId { get; set; }
    }
}
