using System.Text.Json.Serialization;

namespace Hot_desk_booking_system.Models
{
	public class Reservation
	{
		public int Id { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }

		[JsonIgnore]
		public virtual Desk Desk { get; set; }

		public int DeskId { get; set; }
		public int UserId { get; set; }
	}
}
