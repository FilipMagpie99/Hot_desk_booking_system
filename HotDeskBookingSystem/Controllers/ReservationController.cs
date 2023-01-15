using Hot_desk_booking_system.Data;
using Hot_desk_booking_system.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Hot_desk_booking_system.Repositories;
using Hot_desk_booking_system.Services;
using Microsoft.EntityFrameworkCore;

namespace Hot_desk_booking_system.Controllers;

public static class ReservationController
{
    public static void MapReservationEndpoints (this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/Reservation",
			[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
		async (DataContext db) =>
        {
			return await db.Reservation.ToListAsync();
		})
        .WithName("GetAllReservations");

        routes.MapGet("/api/Reservation/{Id}",
			[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator, Standard")]
		async (int Id, DataContext db, IReservationService reservationService) =>
        {
			var reservation = await reservationService.GetReservationByIdAsync(Id);
			if (reservation == null)
			{
				return Results.NotFound();
			}
			return Results.Ok(reservation);
		})
        .WithName("GetReservationById");

		routes.MapPut("/api/Reservation/{Id}",
			[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator, Standard")]
		async (int Id, Reservation reservation, DataContext db, IReservationService reservationService) =>
		{
			if (!await reservationService.UpdateReservation(Id, reservation, db))
			{
				return Results.BadRequest("Error: Desk cannot be changed less than 24 hours before the reservation.");
			}
            else
			    return Results.NoContent();
		})
		.WithName("UpdateReservation");

		routes.MapPost("/api/Reservation/",
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator, Standard")]
		async (int deskId, DateTime startDate, DateTime endDate, IReservationService reservationService) =>
        {
			var reservation = await reservationService.CreateReservation(deskId, startDate, endDate);
			return Results.Created($"/Reservations/{reservation.Id}", reservation);

		})
        .WithName("CreateReservations");

        routes.MapDelete("/api/Reservation/{Id}",
			[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator, Standard")] 
		async (int Id, DataContext db, IReservationService reservationService) =>
        {
			try
			{
			await reservationService.DeleteReservationAsync(Id);
				return Results.NoContent();
			}
			catch (ArgumentException)
			{
				return Results.NotFound();
			}
		})
        .WithName("DeleteReservation");
    }
}
