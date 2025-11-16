using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Threading.Tasks;

namespace BlackJackTests
{
    public class GameControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _httpClient;

        public GameControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _httpClient = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
            });
        }

        [Fact]
        public async Task Index_Returns_PlayPage()
        {
            var response = await _httpClient.GetAsync("/Game/Index");

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            Assert.Contains("Play", content);
        }

        [Fact]
        public async Task Start_Post_InitializesGameAndRedirectsToGame()
        {
            var response = await _httpClient.PostAsync("/Game/Start", null);

            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/Game/Game", response.Headers.Location?.OriginalString);
        }

        [Fact]
        public async Task Game_GetWithoutSession_RedirectsToIndex()
        {
            var response = await _httpClient.GetAsync("/Game/Game");

            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/", response.Headers.Location?.OriginalString);
        }

    }
}
