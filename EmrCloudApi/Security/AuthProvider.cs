using EmrCloudApi.Configs.Options;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EmrCloudApi.Security
{
    public static class AuthProvider
    {
        #pragma warning disable CS8618
        private static JwtOptions _settingInfor;
#       pragma warning restore CS8618 

        /// <summary>
        /// Initializer Auth
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        public static void SetupAuthentication(this IServiceCollection services, ConfigurationManager config)
        {
            _settingInfor = config.GetSection(JwtOptions.Position).Get<JwtOptions>();
            services.AddTransient<IUserService, UserService>();
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = false;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_settingInfor.Secret)),
                    ClockSkew = TimeSpan.Zero
                };
                x.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            context.Response.Headers.Add("Token-Expired", "expired");
                        return Task.CompletedTask;
                    }
                };
            });
        }

        /// <summary>
        /// Create Access Token
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public static string GenerateAccessToken(ICollection<Claim> claims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_settingInfor.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                //Expires = DateTime.UtcNow.AddMinutes(1),
                Expires = DateTime.UtcNow.AddHours(_settingInfor.TokenExpires),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Create Access Token
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public static (string token, DateTime tokenExpiryTime) GenerateAppToken(ICollection<Claim> claims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_settingInfor.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddYears(_settingInfor.AppTokenExpires),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new (tokenHandler.WriteToken(token), tokenDescriptor.Expires.Value);
        }

        /// <summary>
        /// Create Refresh Token
        /// </summary>
        /// <returns></returns>
        public static string GeneratorRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        /// <summary>
        /// Get Info from token is expired time
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settingInfor.Secret)),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
                var jwtSecurityToken = securityToken as JwtSecurityToken;
                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    return null;
                return principal;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Get time valid refresh token
        /// </summary>
        /// <returns></returns>
        public static int GetHoursRefreshTokenExpiryTime() => _settingInfor.RefreshTokenExpires;
    }
}
