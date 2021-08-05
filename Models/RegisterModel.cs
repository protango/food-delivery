using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FoodDelivery.Models
{
    public class RegisterModel : LoginModel
    {
        public string Role { get; set; }

        public RegisterModel(string username, string password, string role): base(username, password)
        {
            Role = role;
        }
    }
}
