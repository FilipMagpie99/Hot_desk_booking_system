using Hot_desk_booking_system.Data;
using Hot_desk_booking_system.Models;
using Hot_desk_booking_system.Repositories;

namespace Hot_desk_booking_system.Services
{
    public interface IReservationService
    {
        public Task<Reservation> CreateReservation(int deskId, DateTime startDate, DateTime endDate);
		Task<Reservation> DeleteReservationAsync(int id);
		public Task<bool> UpdateReservation(int Id, Reservation reservation, DataContext db);
		Task<Reservation> GetReservationByIdAsync(int id);

	}
}
