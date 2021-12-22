using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using System.Net.Http;
using System;
using Server.Integration.Tests;
using Xunit;

namespace Server.Integration.Tests
{
    public class UserTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory _factory;

        public UserTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
            });
        }

        [Fact]
        public async Task Delete_given_id_returns_id_and_success()
        {
            var id = 1;
            var response = await _client.DeleteAsync($"api/users/{id}");
            response.EnsureSuccessStatusCode();
            var deletedId = await response.Content.ReadFromJsonAsync<int>();
            Assert.Equal(1, deletedId);
        }
        
        [Fact]
        public async Task Post_returns_detailsdto_and_success()
        {
             var user = new UserCreateDTO { UserName = "Hanne", UserType = "Student", Email = "Kleppert@live.dk" };
             var response = await _client.PostAsJsonAsync("api/users", user);
             response.EnsureSuccessStatusCode();
             var created = await response.Content.ReadFromJsonAsync<UserDetailsDTO>();
             Assert.NotNull(created);
             Assert.Equal(6, created.Id);
             Assert.Equal("Hanne", created.UserName);
             Assert.Equal("Kleppert@live.dk", created.Email);
             Assert.Equal("Student", created.UserType);
        }

        [Fact]
        public async Task GetByMail_returns_dto()
        {
            var mail = "Roboto@university.com";
            var user = await _client.GetFromJsonAsync<UserDetailsDTO>($"api/users/mail/{mail}");
            Assert.NotNull(user);
            Assert.Equal("Roberto O'boto", user.UserName);
            Assert.Equal("Supervisor", user.UserType);
            Assert.Equal(4, user.Id);
        }

    }
}