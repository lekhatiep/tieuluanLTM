﻿using System.Text.Json.Serialization;

namespace API.Dto.Auth
{
    public class AuthReponseDto
    {
        [JsonPropertyName("access_token")]
        public string Token { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        [JsonPropertyName("experies_in")]
        public int TotalSecond { get; set; }
    }
}
