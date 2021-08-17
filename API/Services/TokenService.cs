using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class TokenService : ITokenService
    {

        //this means private encrpytion, it uses only one key for encryption and decryption.
        private readonly SymmetricSecurityKey _key; 

        public TokenService(IConfiguration config)
        {
          _key=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }

        public string CreateToken(AppUser user)
        {
           var claims=new List<Claim>
           {
               //new Claim(JwtRegisteredClaimNames.NameId, user.UserName) //claim of nameid with the value of the user's username
               new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),//claim of nameid with the value of the user's Id
               new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName) //claim of nameid with the value of the user's username
           };
            //pass in the private key and the strongest algorithm which is HmacSha512
           var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

           var tokenDescriptor = new SecurityTokenDescriptor
           {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials=creds                
           };

           var tokenHandler= new JwtSecurityTokenHandler();

           var token = tokenHandler.CreateToken(tokenDescriptor);

           return tokenHandler.WriteToken(token);
        }
    }
}