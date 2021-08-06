using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FoodDelivery
{
    public static class Utilities
    {
        public static string ExtractUserId(ClaimsPrincipal user) {
            var idClaim = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            if (idClaim == null)
                throw new Exception("ClaimsPrincipal does not contain a NameIdentifier claim");
            return idClaim.Value;
        }
    }
}
