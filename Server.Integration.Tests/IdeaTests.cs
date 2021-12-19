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
    }


}