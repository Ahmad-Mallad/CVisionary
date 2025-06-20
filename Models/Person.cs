using Microsoft.AspNetCore.Identity;

namespace CVisionary.Models
{
    public class Person:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
