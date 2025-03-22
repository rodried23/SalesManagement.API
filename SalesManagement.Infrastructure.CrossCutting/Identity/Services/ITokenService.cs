using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace SalesManagement.Infrastructure.CrossCutting.Identity.Services
{
    public interface ITokenService
    {
        string GenerateToken(Guid userId, string userName, IEnumerable<string> roles);
        ClaimsPrincipal GetPrincipalFromToken(string token);
    }
}