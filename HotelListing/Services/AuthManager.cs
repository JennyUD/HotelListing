using HotelListing.Data;
using HotelListing.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HotelListing.Services
{
    public class AuthManager : IAuthManager
    {
        private readonly UserManager<ApiUser>_UserManager;
        private readonly IConfiguration _IConfiguration;
        private ApiUser  _ApiUser;
        public AuthManager(UserManager<ApiUser>userManager, IConfiguration configuration)
        {
            _UserManager = userManager;
            _IConfiguration= configuration;
        }

        public async Task<string> CreateToken()
        {
            var signInCredentials = GetSigningCredentials();
            var Claims = await GetClaims();
            var Token = GenerateTokenOptions (signInCredentials, Claims);
            return new JwtSecurityTokenHandler().WriteToken(Token);
        }

      

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signInCredentials, List<Claim> claims)
        {
            var jwtSettings = _IConfiguration.GetSection("Jwt");
            var Expiration = DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings.GetSection("Lifetime").Value));
            var Token = new JwtSecurityToken(

                issuer: jwtSettings.GetSection("validIssuer").Value,
                claims: claims,
                expires: Expiration,
                signingCredentials: signInCredentials
                );

            return Token;
        }

       

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
         {
             new Claim(ClaimTypes.Name, _ApiUser.UserName)
         };
            var Roles = await _UserManager.GetRolesAsync(_ApiUser);
            foreach (var GRole in Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, GRole));
            }
            return claims;
        }


        private static SigningCredentials GetSigningCredentials()
        {
            var Key = Environment.GetEnvironmentVariable("OfficeKEY");
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        public  async Task<bool> ValidateUser(LoginUserDTO userDTO)
        {
            _ApiUser = await _UserManager.FindByNameAsync(userDTO.Email);
            return _ApiUser!= null && await _UserManager.CheckPasswordAsync(_ApiUser, userDTO.Password);
        }

        public Task<IQueryable<T>> GetAllUsers()
        {
            IQueryable<T> Query = (IQueryable<T>) _UserProfile;
            if (Query != null)
            {
                //Query.Select<T>(UserProfile);
                var user = await Query.ToListAsync();
                return (IQueryable<T>)user;
            }
            return Query;
        }
    }
}
