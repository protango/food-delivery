using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FoodDelivery.Db
{
    public class Block
    {
        public string BlockingUserId { get; set; } = "";

        public string BlockedUserId { get; set; } = "";

        [JsonIgnore]
        [ForeignKey("BlockingUserId")]
        [InverseProperty("OutboundBlocks")]
        public User? BlockingUser { get; set; }

        [JsonIgnore]
        [ForeignKey("BlockedUserId")]
        [InverseProperty("InboundBlocks")]
        public User? BlockedUser { get; set; }

        public string? BlockedUsername { get => BlockedUser?.UserName; }
    }
}