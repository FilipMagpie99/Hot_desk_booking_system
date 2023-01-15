using Hot_desk_booking_system.Data;
using Microsoft.EntityFrameworkCore;
using Hot_desk_booking_system.Controllers;
using Microsoft.OpenApi.Models;
using Hot_desk_booking_system.Models;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Hot_desk_booking_system.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using IHot_desk_booking_system.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
	options.TokenValidationParameters = new TokenValidationParameters()
	{
		ValidateActor= true,
		ValidateAudience= true,
		ValidateLifetime= true,
		ValidateIssuerSigningKey= true,
		ValidIssuer = builder.Configuration["Jwt:Issuer"],
		ValidAudience = builder.Configuration["Jwt:Audience"],
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
	};
});
builder.Services.AddAuthorization();

builder.Services.AddSwaggerGen(options =>
{
	options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Scheme = "Bearer",
		BearerFormat = "JWT",
		In = ParameterLocation.Header,
		Name = "Authorization",
		Description = "Bearer Authentication with JWT Token",
		Type = SecuritySchemeType.Http
	});
	options.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Id = "Bearer",
					Type = ReferenceType.SecurityScheme
				}
			},
			new List<string>()
		}
	});
});

builder.Services.AddDbContext<DataContext>(options =>
{
	options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();
app.MapPost("/login", (UserLogin user, IUserService service) => Login(user, service)).WithName("Login");
IResult Login(UserLogin user, IUserService service)
{
	if (!string.IsNullOrEmpty(user.Username) &&
		!string.IsNullOrEmpty(user.Password))
	{
		var loggedInUser = service.Get(user);
		if (loggedInUser is null) return Results.NotFound("User not found");

		var claims = new[]
	   {
			new Claim(ClaimTypes.NameIdentifier, loggedInUser.Username),
			new Claim(ClaimTypes.Email, loggedInUser.EmailAdress),
			new Claim(ClaimTypes.GivenName, loggedInUser.GivenName),
			new Claim(ClaimTypes.Surname, loggedInUser.Surname),
			new Claim(ClaimTypes.Role, loggedInUser.Role)
		};

		token = new JwtSecurityToken
		(
			issuer: builder.Configuration["Jwt:Issuer"],
			audience: builder.Configuration["Jwt:Audience"],
			claims: claims,
			expires: DateTime.UtcNow.AddDays(60),
			notBefore: DateTime.UtcNow,
			signingCredentials: new SigningCredentials(
				new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
				SecurityAlgorithms.HmacSha256)
		);

		var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
		return Results.Ok(tokenString);
	}
	return Results.BadRequest("Invalid user credentials");
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
app.UseAuthentication();

app.UseAuthorization();


app.UseHttpsRedirection();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapDeskEndpoints();

app.MapLocationEndpoints();

app.MapReservationEndpoints();

app.Run();

public partial class Program
{
	public static JwtSecurityToken token { get; set; }
}