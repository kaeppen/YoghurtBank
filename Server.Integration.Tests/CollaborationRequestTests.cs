using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http.Json;
using System.Net.Http;
using System;
using Xunit;

namespace Server.Integration.Tests
{
    public class CollaborationRequestTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory _factory;

        public CollaborationRequestTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
            });
        }
        

        //er det nok at tjekke statuskode? kan man tjekke returværdi også?
        [Fact]
        public async Task Delete_returns_success_status()
        {
            var id = 1; 
            var response = await _client.DeleteAsync($"api/collaborationrequest/{id}");
            response.EnsureSuccessStatusCode();

        }

        //er det nok at tjekke statuskode? kan man tjekke returværdi også?
        [Fact]
        public async Task Put_returns_success_status()
        {
            var update = new CollaborationRequestUpdateDTO
            {
                Id = 1,
                Status = CollaborationRequestStatus.ApprovedByStudent
            };
            var response = await _client.PutAsJsonAsync($"api/collaborationrequest/{1}", update);
            response.EnsureSuccessStatusCode();
        }

    }
}