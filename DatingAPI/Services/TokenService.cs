using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DatingAPI.Entities;
using DatingAPI.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace DatingAPI.Services
{
    public class TokenService : ITokenService
    {
        //creating a JSON web token that we can return to our clients.
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));            
        }
        public string CreateToken(AppUser user)
        {
            //setting a claim which is a bit of information that a user claims. here we are setting it to UserName.
            var claims = new List<Claim>{
                new Claim(JwtRegisteredClaimNames.NameId , user.UserName)
            };
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}