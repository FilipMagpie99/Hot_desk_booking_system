using Microsoft.EntityFrameworkCore;
using Hot_desk_booking_system.Data;
using Hot_desk_booking_system.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Hot_desk_booking_system.Controllers;

public static class DeskController
{
    public static void MapDeskEndpoints (this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/Desk",
            [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles = "Administrator, Standard")]
            async (DataContext db) =>
        {
            return await db.Desk.ToListAsync();
        })
        .WithName("GetAllDesks");        
               
        
        routes.MapGet("/api/RentedDesk",
			[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
		async (DataContext db) =>
        {
			var rentedDesks = from r in db.Reservation
							  join d in db.Desk on r.DeskId equals d.Id
							  where DateTime.Now >= r.StartDate && DateTime.Now <= r.EndDate
							  select new { d.Id, d.Number };

			return await rentedDesks.ToListAsync();
        })
        .WithName("GetAllDesksWithUsers");        
        

		// Determine which desks are available to book or unavailable.
		routes.MapGet("/api/AvaliableDesk",
			[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator, Standard")]
		    async (DataContext db) =>
        {
            IQueryable<Desk> desks = db.Desk.Where(x => x.IsRented == false);
            return await desks.ToListAsync();
        })
        .WithName("GetAllAvailableDesks");

		//Filter desks based on location
		routes.MapGet("/api/DeskByLocationName",
			[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator, Standard")]
		    async (string? locationName, DataContext db) =>
        {
            var desks = db.Desk.AsQueryable();
            if (!string.IsNullOrEmpty(locationName))
            {
                desks = db.Desk.Where(x => x.Location.Name.ToLower().Contains(locationName.ToLower()));
				return await desks.ToListAsync();
			}
			else
                return new List<Desk>();
        })
        .WithName("GetAllDeskByLocation");


		routes.MapGet("/api/Desk/{id}",
			[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator, Standard")]
		    async (int Id, DataContext db) =>

        {
            return await db.Desk.FindAsync(Id)
                is Desk model
                    ? Results.Ok(model)
                    : Results.NotFound();
        })
        .WithName("GetDeskById");

        routes.MapPut("/api/Desk/{id}",
			[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")] 
        async (int Id, Desk desk, DataContext db) =>
        {
            var foundModel = await db.Desk.FindAsync(Id);

            if (foundModel is null)
            {
                return Results.NotFound();
            }
            
            db.Update(desk);

            await db.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithName("UpdateDesk");

        routes.MapPost("/api/Desk/",
			[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
		async (Desk desk, DataContext db) =>
        {
            db.Desk.Add(desk);
            await db.SaveChangesAsync();
            return Results.Created($"/Desks/{desk.Id}", desk);
        })
        .WithName("CreateDesk");

        routes.MapDelete("/api/Desk/{Id}",
			[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
		async (int Id, DataContext db) =>
        {
            if (await db.Desk.FindAsync(Id) is Desk desk)
            {
                db.Desk.Remove(desk);
                await db.SaveChangesAsync();
                return Results.Ok(desk);
            }

            return Results.NotFound();
        })
        .WithName("DeleteDesk");
    }
}
