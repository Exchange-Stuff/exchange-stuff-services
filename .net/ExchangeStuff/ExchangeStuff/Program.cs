using ExchangeStuff.Exceptions;
using ExchangeStuff.Extensions;

using ExchangeStuff.Service.Extensions;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Serialization;
using Serilog;
using ExchangeStuff.Service.Maps;
using AutoMapper;
using ExchangeStuff.Hubs;
using ExchangeStuff.Service.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options => options.AddPolicy("CorsPolicy",
        builder =>
        {
            builder.AllowAnyHeader()
                   .AllowAnyMethod()
                   .SetIsOriginAllowed((host) => true)
                   .AllowCredentials().WithExposedHeaders("IS-EXCHANGESTUFF-TOKEN-EXPIRED"); ;
        }));

builder.AddLogging();
builder.Services.AddControllers(x =>
{
    x.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
    x.Filters.Add(typeof(ExceptionHandler));
}).AddNewtonsoftJson(x =>
{
    x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    x.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
}).AddJsonOptions(x =>
{
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;

});

builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.InjectAPI(builder.Configuration);
builder.Services.Inject(builder.Configuration);


var app = builder.Build();
app.UseCors("CorsPolicy");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSerilogRequestLogging();
app.UseRouting();
app.UseCors("CorsPolicy");

app.UseAuthentication();

app.UseMiddleware();
app.UseAuthorization();
app.UseException();

app.MapControllers();
app.MapHub<ESNotification>("/esnotification");
app.MapHub<ChatHub>("/chat");
app.Run();
