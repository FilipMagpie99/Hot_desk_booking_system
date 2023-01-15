using System.Text.Json.Serialization;

namespace Hot_desk_booking_system.Models
{
	public class Location
	{
		public int Id { get; set; }
		public string? Name { get; set; }

		[JsonIgnore]
		public virtual ICollection<Desk> Desks { get; set; }
	}
}
