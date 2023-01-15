using Hot_desk_booking_system.Models;

namespace Hot_desk_booking_system.Repositories
{
	public class UserRepository
	{
		public static List<User> Users = new()
		{
			new() {Id = 1,Username = "admin", EmailAdress="admin@admin.com", Password="admin",GivenName="admin",Surname="admin",Role="Administrator"},
			new() {Id = 2,Username = "user", EmailAdress="user@user.com", Password="user",GivenName="user",Surname="user",Role="Standard"}
		};
	}
}
