using Microsoft.EntityFrameworkCore;
using Hot_desk_booking_system.Data;
using Hot_desk_booking_system.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Hot_desk_booking_system.Services;

namespace Hot_desk_booking_system.Controllers;

public static class LocationController
{
    public static void MapLocationEndpoints (this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/Location",
			[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator, Standard")]
		    async (DataContext db) =>
        {
			return await db.Location.ToListAsync();
		})
        .WithName("GetAllLocations");

        routes.MapGet("/api/Location/{Id}", 
            [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator, Standard")] 
            async (int Id, DataContext db, ILocationService locationService) =>

			{
				return await db.Location.FindAsync(Id)
					is Location model
						? Results.Ok(model)
						: Results.NotFound();
			})
		.WithName("GetLocationById");

		routes.MapPut("/api/Location/{Id}",
			[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
		async (int Id, Location location, DataContext db, ILocationService locationService) =>
        {
			if (!await locationService.UpdateLocation(Id, location, db))
			{
				return Results.BadRequest("Error: Location not found.");
			}
            else
			    return Results.NoContent();
        })
        .WithName("UpdateLocation");

      
        routes.MapPost("/api/Location/",
		    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
		    async (Location location, DataContext db, ILocationService locationService) =>
        { 
            if(!await locationService.CreateLocation(location))
            {
				return Results.BadRequest("Error: Location was empty.");
			}
            else
                return Results.Ok();
		})
        .WithName("CreateLocation");

        routes.MapDelete("/api/Location/{Id}",
		    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
		    async (int Id, DataContext db, ILocationService locationService) =>
        {
			try
			{
				await locationService.DeleteLocationAsync(Id);
				return Results.NoContent();
			}
			catch (ArgumentException)
			{
				return Results.NotFound();
			}
		})
        .WithName("DeleteLocation");
    }
}
