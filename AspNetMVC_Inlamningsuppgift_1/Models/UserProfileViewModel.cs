namespace AspNetMVC_Inlamningsuppgift_2.Models
{
    public class UserProfileViewModel
    {

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";

        public string Email { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string Zipcode { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string PageRole { get; set; } = string.Empty;

        public string ProfilePicture { get; set; } = string.Empty; 
    }
}
