using Hot_desk_booking_system.Data;
using Hot_desk_booking_system.Models;
using Hot_desk_booking_system.Repositories;
using System.Data.Entity;
using System.Security.Claims;

namespace Hot_desk_booking_system.Services
{
	public class ReservationService : IReservationService
	{
		private DataContext _db;

		public ReservationService(DataContext db)
		{
			_db = db;
		}

		public async Task<Reservation> CreateReservation(int deskId, DateTime startDate, DateTime endDate)
		{
			var desk = await _db.Desk.FindAsync(deskId);
			if (desk == null || desk.IsRented)
			{
				throw new Exception("Error: Desk not found or desk is rented");
			}
			if (endDate - startDate > TimeSpan.FromDays(7))
			{
				throw new Exception("Error: Reservation can last 7 days max.");
			}
			if (endDate < DateTime.Now || startDate < DateTime.Now || endDate < startDate)
			{
				throw new Exception("Error: Date must be in future.");
			}
			Reservation reservation = new Reservation
			{
				DeskId = deskId,
				StartDate = startDate,
				EndDate = endDate,
			};
			var jwtToken = Program.token;
			var currentPrincipal = jwtToken.Claims;
			if (currentPrincipal.Any())
			{
				var usernameClaim = currentPrincipal.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
				if (usernameClaim != null)
				{
					int userId = UserRepository.Users.Single(x => x.Username.Equals(usernameClaim.Value)).Id;
					reservation.UserId = userId;
				}
			}
			desk.IsRented = true;
			_db.Reservation.Add(reservation);
			await _db.SaveChangesAsync();
			return reservation;
		}

		public async Task<bool> UpdateReservation(int Id, Reservation reservation, DataContext db)
		{
			var foundModel = await _db.Reservation.FindAsync(Id);
			if (foundModel is null)
			{
				return false;
			}
			if (foundModel.EndDate - DateTime.Now < TimeSpan.FromHours(24))
			{
				return false;
			}

			foundModel.DeskId = reservation.DeskId;
			_db.Update(reservation);
			await _db.SaveChangesAsync();
			return true;
		}
		public async Task<Reservation> DeleteReservationAsync(int id)
		{
			var reservation = await _db.Reservation.FindAsync(id);
			if (reservation != null)
			{
				reservation.Desk.IsRented = false;
				_db.Reservation.Remove(reservation);
				await _db.SaveChangesAsync();
				return reservation;
			}
			else
				throw new ArgumentException("Reservation not found");
		}

		public async Task<Reservation> GetReservationByIdAsync(int id)
		{
			return await _db.Reservation.FindAsync(id);
		}

	}
}
