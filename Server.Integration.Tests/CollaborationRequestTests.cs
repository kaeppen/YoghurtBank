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
        
        [Fact]
        public async Task Delete_returns_success_status()
        {
            var id = 1; 
            var response = await _client.DeleteAsync($"api/collaborationrequest/{id}");
            response.EnsureSuccessStatusCode();
            var deletedId = await response.Content.ReadFromJsonAsync<int>();
            Assert.Equal(1, deletedId);

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
            var updated = await response.Content.ReadFromJsonAsync<CollaborationRequestDetailsDTO>();
            Assert.Equal(CollaborationRequestStatus.ApprovedByStudent, updated.Status);
        }

        [Fact]
        public async Task Post_returns_success_status()
        {
            var cb = new CollaborationRequestCreateDTO
            {
                StudentId = 1,
                SupervisorId = 4,
                Application = "Science",
                IdeaId = 1
            };
            var response = await _client.PostAsJsonAsync($"api/collaborationrequest", cb);
            response.EnsureSuccessStatusCode();
            var created = await response.Content.ReadFromJsonAsync<CollaborationRequestDetailsDTO>();
            Assert.Equal(7, created.Id);
            Assert.Equal("Science", created.Application);
            Assert.Equal(CollaborationRequestStatus.Waiting, created.Status);
            Assert.Equal(1, created.StudentId);
            Assert.Equal(4, created.SupervisorId);
        }

        [Fact]
        public async Task GetByUserId_given_1_returns_4_or_more_requests()
        {
            var studentId = 1;
            var response = await _client.GetFromJsonAsync<IReadOnlyCollection<CollaborationRequestDetailsDTO>>($"api/collaborationrequest/id/{studentId}");
            Assert.NotNull(response);
            Assert.NotEmpty(response);
            Assert.True(response.Count() >= 4);
        }

    }
}