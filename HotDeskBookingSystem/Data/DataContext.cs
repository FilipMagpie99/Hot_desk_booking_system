using Hot_desk_booking_system.Models;
using Microsoft.EntityFrameworkCore;

namespace Hot_desk_booking_system.Data
{
    public class DataContext : DbContext
	{
		public DataContext()
		{
		}

		public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Desk> Desk { get; set; }

        public DbSet<Location> Location { get; set; }

        public DbSet<Reservation> Reservation { get; set; }


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Location>().HasMany(x => x.Desks).WithOne(d => d.Location).HasForeignKey(d => d.LocationId).OnDelete(DeleteBehavior.Restrict);
		}




	}
}
