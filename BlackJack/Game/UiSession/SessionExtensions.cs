using System.Text.Json;
using BlackJack.DataTransfer.GameDtos;
using Microsoft.AspNetCore.Http;


namespace BlackJack.Game.UiSession
{
    public static class SessionExtensions
    {
        private static readonly JsonSerializerOptions _options = new()
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
        };

        public static void SetGameState(this ISession session, string key, GameStateDto state)
        {
            session.SetString(key, JsonSerializer.Serialize(state, _options));
        }

        public static GameStateDto? GetGameState(this ISession session, string key)
        {
            var s = session.GetString(key);
            return s is null ? null : JsonSerializer.Deserialize<GameStateDto>(s, _options);
        }
    }
}
