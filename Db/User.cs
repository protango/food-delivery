using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text.Json.Serialization;

namespace FoodDelivery.Db
{
    public class User : IdentityUser
    {
        [JsonIgnore]
        public ICollection<Order>? Orders { get; set; }
        [JsonIgnore]
        public ICollection<Restaurant>? Restaurants { get; set; }
        [JsonIgnore]
        public ICollection<Block>? OutboundBlocks { get; set; }
        [JsonIgnore]
        public ICollection<Block>? InboundBlocks { get; set; }
    }
}