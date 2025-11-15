using System.Text.Json;
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

        public static void SetUiState(this ISession session, string key, UiState state)
        {
            session.SetString(key, JsonSerializer.Serialize(state, _options));
        }

        public static UiState? GetUiState(this ISession session, string key)
        {
            var s = session.GetString(key);
            return s is null ? null : JsonSerializer.Deserialize<UiState>(s, _options);
        }
    }
}
