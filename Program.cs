using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//Bind JwtOptions form appsettings.json
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));

// Register JwtGenerator and IJwtGenerator
builder.Services.AddSingleton<IJwtGenerator, JwtGenerator>();

// Register authentication service
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtOptions:Secret"]!)),
            ValidIssuer = builder.Configuration["JwtOptions:Issuer"],
            ValidAudience = builder.Configuration["JwtOptions:Audience"]
        };
    });

// Enabl middleware that validates the incomming token based on the above configuraiton.
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

// Add controllers to services. 
builder.Services.AddControllers();

var app = builder.Build();

// Map controllers
app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    var options = builder.Configuration.GetSection("JwtOptions").Get<JwtOptions>();
    System.Console.WriteLine($"Secret: {options?.Secret} and issuer: {options?.Issuer}");
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

// Minimal API route
app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

