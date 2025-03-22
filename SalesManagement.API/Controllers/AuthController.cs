using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SalesManagement.API.Models;
using SalesManagement.Infrastructure.CrossCutting.Identity.Services;

namespace SalesManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public AuthController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] Models.LoginRequest model)
        {
            // For demo purposes - in a real application, validate credentials against the database
            if (model.Username == "admin" && model.Password == "admin")
            {
                var token = _tokenService.GenerateToken(Guid.NewGuid(), model.Username, new[] { "Admin" });
                return Ok(new { token });
            }

            return Unauthorized(new ErrorResponse
            {
                Type = "AuthenticationError",
                Error = "Invalid credentials",
                Detail = "The provided username or password is incorrect"
            });
        }
    }
}