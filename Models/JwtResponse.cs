using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FoodDelivery.Models
{
    public class JwtResponse
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }

        public JwtResponse(string token, DateTime expiration) {
            Token = token;
            Expiration = expiration;
        }
    }
}
