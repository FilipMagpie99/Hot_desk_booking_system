using System.IdentityModel.Tokens.Jwt;
using System.ComponentModel.DataAnnotations;
namespace Hot_desk_booking_system.Models
{
    public class UserLogin
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

    }
}
