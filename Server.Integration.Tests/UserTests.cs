using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using System.Net.Http;
using System;
using Xunit;

namespace Server.Integration.Tests
{
    public class UserTests 
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
    }
}