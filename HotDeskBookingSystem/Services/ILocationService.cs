using Hot_desk_booking_system.Data;
using Hot_desk_booking_system.Models;
using Hot_desk_booking_system.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Hot_desk_booking_system.Services
{
    public interface ILocationService
    {
		Task<Location> GetLocationByIdAsync(int id);
		Task<Location> DeleteLocationAsync(int id);
		public Task<bool> UpdateLocation(int Id, Location location, DataContext db);
		public Task<bool> CreateLocation(Location location);
	}
}
