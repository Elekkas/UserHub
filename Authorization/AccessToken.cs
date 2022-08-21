using Authorization.DataTransferObjects;
using Authorization.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Authorization
{
    public class AccessToken : IAccessToken
    {
        private const int TokenIsValidFor = 1;
        private readonly ICryptography _cryptoServise;


        public AccessToken(ICryptography cryptoServise)
        {
            _cryptoServise = cryptoServise;
        }

        public string CreateAccessToken(UserDTO user)
        {
            var header = ToBase64(JsonSerializer.Serialize(new
            {
                alg = "HS256",
                typ = "JWT"
            }));

            var body = ToBase64(JsonSerializer.Serialize(new
            {
                iss = "UserHub",
                exp = DateTime.Now.AddHours(TokenIsValidFor).ToString("G"),
                usr = user.Username,
                rol = user.Role,
                sub = user.Email
            }));

            var signature = _cryptoServise.Hash($"{header}.{body}");

            return $"{header}.{body}.{signature}";
        }


        public string GetName(string token) =>
            JObject.Parse(DecodeBody(token)).Value<string>("usr");

        public string GetRole(string token) =>
            JObject.Parse(DecodeBody(token)).Value<string>("rol");

        public string GetEmail(string token) =>
            JObject.Parse(DecodeBody(token)).Value<string>("sub");

        private string ToBase64(string message) =>
            Convert.ToBase64String(Encoding.UTF8.GetBytes(message));

        private string FromBase64(string base64Message) =>
            Encoding.UTF8.GetString(Convert.FromBase64String(base64Message));

        public bool IsTokenValid(string token) =>
            IsSignatureUntampered(token) && !IsTokenExpired(token);

        private bool IsSignatureUntampered(string token)
        {
            var tokenParts = token.Split('.');

            if (tokenParts.Length != 3)
            {
                return false;
            }

            return _cryptoServise.VerifyHash(tokenParts[2], $"{tokenParts[0]}.{tokenParts[1]}");
        }

        private bool IsTokenExpired(string token) =>
            DateTime.Parse(JObject.Parse(DecodeBody(token)).Value<string>("exp")) < DateTime.Now;

        private string DecodeBody(string token) =>
            FromBase64(token.Split('.')[1]);
    }
}
