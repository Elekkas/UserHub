using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Authorization.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Authorization
{
    public class Cryptography : ICryptography
    {
        private string _secret;

        public Cryptography(IConfiguration configuration)
        {
            _secret = configuration.GetSection("Secret").Value;
        }

        public string Hash(string message)
        {
            var encoding = new ASCIIEncoding();
            byte[] keyBytes = encoding.GetBytes(_secret);
            byte[] messageBytes = encoding.GetBytes(message);

            var hMacSha256Provider = new HMACSHA256(keyBytes);
            byte[] hashedMessage = hMacSha256Provider.ComputeHash(messageBytes);

            var hashedBase64 = Convert.ToBase64String(hashedMessage);

            return hashedBase64;
        }

        public bool VerifyHash(string hash, string message) => Hash(message) == hash;
    }
}
