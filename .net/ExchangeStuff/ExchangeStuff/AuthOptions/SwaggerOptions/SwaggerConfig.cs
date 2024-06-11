using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace ExchangeStuff.AuthOptions.SwaggerOptions
{
    public static class SwaggerConfig
    {
        public static void AddSwaggerES(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(c =>
            {
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
                    $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));

                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Exchange Stuff Application",
                    Description = "Exchange Stuff API Service Interface",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                    {
                        Email = "nguyenminhvu.august@gmail.com",
                        Name = "Exchange Stuff",
                        Url = new Uri("https://github.com/Exchange-Stuff")
                    },
                    Version = "v1"
                });
                c.DescribeAllParametersInCamelCase();
                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.",
                    In = ParameterLocation.Header,
                    Scheme = JwtBearerDefaults.AuthenticationScheme
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme{
                        Scheme="oauth2",
                        Name=JwtBearerDefaults.AuthenticationScheme,
                        Reference= new OpenApiReference
                        {
                        Type=ReferenceType.SecurityScheme,
                        Id=JwtBearerDefaults.AuthenticationScheme
                        }
                    },new List<string>()
                }
                });
            });
        }
    }
}
