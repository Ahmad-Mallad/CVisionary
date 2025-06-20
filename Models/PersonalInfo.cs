using Microsoft.CodeAnalysis.CodeActions;

namespace CVisionary.Models
{
    public class PersonalInfo
    {
        public string FirstName { get; set; }
        public string ?SecondName { get; set; }
        public string ?ThirdName { get; set; }
        public string LastName { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string? LinkedInLink { get; set; }
        public string? GithubLink { get; set; }
        public string? FacebookLink { get; set; }
        public string? InstagramLink { get; set; }

        public string? Address { get; set; }

        public string? DateOfBirth { get; set; }

        public string? Summary { get; set; }

        public string ?Title { get; set; }


    }
}
