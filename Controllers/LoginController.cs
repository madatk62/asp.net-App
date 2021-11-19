using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using demo2.Models;
using Microsoft.AspNetCore.Authorization;
using demo2.Data;
namespace demo2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config; // to get key, issuer from appsettings.json
        private readonly demo2Context _context;
        public LoginController(IConfiguration config, demo2Context context) //Declare DI
        {
            _config = config;
            _context = context;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] Account account)
        {
            IActionResult response = Unauthorized();
            bool checkedAccount = AuthenticateUser(account);
            if (checkedAccount)
            {
                string jwt = getAccessToken();
                response = Ok(new { token = jwt });
            }
            return response;
        }
        private string getAccessToken()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
 
            var claims = new List<Claim>();

            claims.Add(new Claim("username", "123abc"));
            claims.Add(new Claim("password", "123abc"));
            // create credentials
            var secret_key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
            var symmetricSecurityKey = new SymmetricSecurityKey(secret_key);
            var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

            // create token by handler
            var access_token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(3),
                signingCredentials: credentials
                );

            return tokenHandler.WriteToken(access_token);
        }
        private bool AuthenticateUser(Account user)
        {
            var existUser = _context.Account.Where(p=>p.Username == user.Username && p.Password == user.Password).Take(1).Any();
            if (existUser)
            {
                return true;
            }
            return false;
        }
    }
}