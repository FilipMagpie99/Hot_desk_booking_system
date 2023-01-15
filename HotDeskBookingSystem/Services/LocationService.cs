using Hot_desk_booking_system.Data;
using Hot_desk_booking_system.Models;
using Hot_desk_booking_system.Repositories;
using Hot_desk_booking_system.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;

namespace IHot_desk_booking_system.Services
{
	public class LocationService : ILocationService
	{
		private DataContext _db;
		public LocationService(DataContext db)
		{
			_db = db;
		}

		public async Task<bool> CreateLocation(Location location)
		{
			if(location == null)
			{
				return false;
			}
			_db.Location.Add(location);
			await _db.SaveChangesAsync();
			return true;
		}

		public async Task<Location> DeleteLocationAsync(int id)
		{
			var location = await _db.Location.FindAsync(id);
			if (location != null)
			{
				_db.Location.Remove(location);
				await _db.SaveChangesAsync();
				return location;
			}
			else
				throw new ArgumentException("Reservation not found");
		}

		public async Task<Location> GetLocationByIdAsync(int id)
		{
			return await _db.Location.FindAsync(id);
		}

		public async Task<bool> UpdateLocation(int Id, Location location, DataContext db)
		{
			var foundModel = await _db.Location.FindAsync(Id);
			if (foundModel is null)
			{
				return false;
			}
			foundModel.Name = location.Name;
			_db.Update(location);
			await _db.SaveChangesAsync();
			return true;
		}

	}
}
