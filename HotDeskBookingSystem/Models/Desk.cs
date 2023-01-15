using System.Text.Json.Serialization;

namespace Hot_desk_booking_system.Models
{
	public class Desk
	{
		public int Id { get; set; }
		public int Number { get; set; }
		public bool IsRented { get; set; }
		[JsonIgnore]
		public virtual Location Location { get; set; }
		public int LocationId { get; set; }

	}
}
