using System.Text.Json.Serialization;

namespace WebClient.Models
{
    public class PlayerModel
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

        [JsonPropertyName("inGameName")]
        public string InGameName { get; set; }

        [JsonPropertyName("elo")]
        public int Elo { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("banned")]
        public bool Banned { get; set; }

        [JsonPropertyName("currencyAmount")]
        public int CurrencyAmount { get; set; }

        [JsonPropertyName("onlineStatus")]
        public bool OnlineStatus { get; set; }

        [JsonPropertyName("birthday")]
        public DateTime Birthday { get; set; }
    }
}
