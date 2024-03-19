using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateActor = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        RequireExpirationTime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration.GetSection("Jwt:Issuer").Value,
        ValidAudience = builder.Configuration.GetSection("Jwt:Audience").Value,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:SecretKey").Value))
    };
});
builder.Services.AddAuthorization();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

var roles = new Dictionary<string, string>{
    {"admin","Administrator"},
};
app.MapPost("/login", async ([FromBody] LoginRequest request, IConfiguration config) =>
{
    var claims = new List<Claim>();
    if (roles.TryGetValue(request.UserName, out var role))
    {
        claims.Add(new Claim("Role", role));
    }
    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("Jwt:SecretKey").Value));
    var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
    var sercurityToken = new JwtSecurityToken(
        claims: claims,
        expires: DateTime.Now.AddMinutes(5),
        issuer: config.GetSection("Jwt:Issuer").Value,
        audience: config.GetSection("Jwt:Audience").Value,
        signingCredentials: signingCredentials
        );
    var tokenString = new JwtSecurityTokenHandler().WriteToken(sercurityToken);
    return tokenString;
})
.WithOpenApi();

app.Run();

public class LoginRequest
{
    public string UserName { get; set; }
    public string Password { get; set; }
}