using System.IdentityModel.Tokens.Jwt;
using System.ComponentModel.DataAnnotations;
namespace Hot_desk_booking_system.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string Username { get; set; }

        public string EmailAdress { get; set; }

        public string Password { get; set; }

        public string GivenName { get; set; }

        public string Surname { get; set; }

        public string Role {get; set;}

    }

}
