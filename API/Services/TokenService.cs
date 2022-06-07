using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.IO.Enumeration;
using System.IO;
using System.Security.AccessControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Services;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace API.Services
{
    public class TokenService : ITokenService
    {
        // Symmertic encruption is when only 1 key is used to 
        // encrypt and decrypt information
        // Symm keys used to sign token and make sure the signature is verified
        // the other is asymmetric for encrpty and decrypt
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }


        public string CreateToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.UserName)
            };

            // below cretes credentials for token creation

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var TokenDescriptor = new SecurityTokenDescriptor
            {
                // down here speicifes what belongs in our token
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            // this then creates teh token
            // then return returns the token when needed and 
            // created

            var token = tokenHandler.CreateToken(TokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
        
    }
}