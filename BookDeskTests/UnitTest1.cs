using Hot_desk_booking_system.Models;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace BookDeskTests
{
	public class Tests
	{
		private HttpClient _client;

		public static T? Deserialize<T>(string text)
		{

			return JsonSerializer.Deserialize<T>(text, new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			});
		}

		[OneTimeSetUp]
		public async Task SetupAsync()
		{
			_client = new HttpClient();
			UserLogin user = new UserLogin
			{
				Id = 1,
				Username = "admin",
				Password = "admin"
			};
			var login = await _client.PostAsJsonAsync<UserLogin>("https://localhost:7164/login", user);
			var token = await login.Content.ReadAsStringAsync();
			var validToken = token.Replace("\"", "");
			if (login.IsSuccessStatusCode)
			{
				_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", validToken);

			}
		}

		[Test]
		public async Task TestGetDesksAsAdmin()
		{

			var response = await _client.GetAsync("https://localhost:7164/api/Desk");
			var result = await response.Content.ReadAsStringAsync();
			Assert.IsNotEmpty(result);
			
		}
	}
}