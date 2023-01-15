using Hot_desk_booking_system.Data;
using Hot_desk_booking_system.Models;
using Hot_desk_booking_system.Repositories;

namespace Hot_desk_booking_system.Services
{
    public interface IUserService
    {
        public User Get(UserLogin userLogin);
    }
}
