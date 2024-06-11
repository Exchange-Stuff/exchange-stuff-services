using ExchangeStuff.Service.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ExchangeStuff.AuthOptions.Jwts
{
    public static class JwtConfiguration
    {
        public static void AddESJwt(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                JwtDTO jwtDTO = new JwtDTO();
                configuration.GetSection(nameof(JwtDTO)).Bind(jwtDTO);
                o.SaveToken = true; 

                var key = Encoding.UTF8.GetBytes(jwtDTO.JwtSecret);

                o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = false, 
                    ValidateAudience = false, 
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtDTO.Issuer,
                    ValidAudience = jwtDTO.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                };
                o.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = x =>
                    {   
                        if (x.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            x.Response!.Headers!.TryAdd("IS-EXCHANGESTUFF-TOKEN-EXPIRED", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });
            services.AddAuthorization();
        }
    }
}
