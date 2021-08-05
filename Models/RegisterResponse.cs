using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FoodDelivery.Models
{
    public class RegisterResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }

        public RegisterResponse(string status, string message) {
            Status = status;
            Message = message;
        }
    }
}
