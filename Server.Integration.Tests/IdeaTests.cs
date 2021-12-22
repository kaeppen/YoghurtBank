using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http.Json;
using System.Net.Http;
using System;

namespace Server.Integration.Tests;

public class IdeaTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public IdeaTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
        });
    }


    [Fact]
    public async Task Get_returns_ideas()
    {
        var ideas = await _client.GetFromJsonAsync<IdeaDetailsDTO[]>("api/idea");
        Assert.NotNull(ideas);
        Assert.NotEmpty(ideas);
    }

    [Fact]
    public async Task Post_returns_success_code()
    {
        var idea = new IdeaCreateDTO
        {
            CreatorId = 4,
            Title = "GADDAG's and their use in ScrabbleBots",
            Subject = "Scrabble",
            Description = "Heya Sweden",
            AmountOfCollaborators = 2,
            Open = true,
            TimeToComplete = DateTime.UtcNow - DateTime.UtcNow,
            StartDate = DateTime.UtcNow.Date,
            Type = IdeaType.Bachelor
        };

        var response = await _client.PostAsJsonAsync("api/idea", idea);
        response.EnsureSuccessStatusCode();
        var created = await response.Content.ReadFromJsonAsync<IdeaDetailsDTO>();
        Assert.Equal(4, created.CreatorId);
        Assert.Equal("GADDAG's and their use in ScrabbleBots", created.Title);
        Assert.Equal("Scrabble", created.Subject);
        Assert.Equal("Heya Sweden", created.Description);
        Assert.Equal(2, created.AmountOfCollaborators);
        Assert.True(created.Open);
        Assert.Equal(DateTime.UtcNow.Date, created.StartDate);
        Assert.Equal(IdeaType.Bachelor, created.Type);
    }

    [Fact]
    public async Task Delete_returns_success_and_id()
    {
        var id = 1;
        var response = await _client.DeleteAsync($"api/Idea/{id}");
        response.EnsureSuccessStatusCode();
        var deletedId = await response.Content.ReadFromJsonAsync<int>();
        Assert.Equal(1, deletedId);
    }
    
      
    [Fact]
    public async Task GetBySuperId_given_5_returns_3_ideas()
    {
        var id = 5; 
        var ideas = await _client.GetFromJsonAsync<IdeaDetailsDTO[]>($"api/idea/user/{id}");
        Assert.NotNull(ideas);
        Assert.NotEmpty(ideas);
        Assert.Equal(3, ideas.Length);
    }
    
    [Fact]
    public async Task Put_returns_success_and_updatedto()
    {
        var id = 2;
        var update = new IdeaUpdateDTO
        {
            Title = "NewTitle",
            Subject = "NewSubject",
            Description = "NewDescription",
            AmountOfCollaborators = 5,
            Open = false
        };

        var response = await _client.PutAsJsonAsync($"api/idea/{id}", update);
        response.EnsureSuccessStatusCode();
        var updated = await response.Content.ReadFromJsonAsync<IdeaDetailsDTO>();
        Assert.Equal("NewTitle", updated.Title);
        Assert.Equal("NewSubject", updated.Subject);
        Assert.Equal("NewDescription", updated.Description);
        Assert.Equal(5, updated.AmountOfCollaborators);
        Assert.False(updated.Open);
    }


}