using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
public sealed class JwtGenerator : IJwtGenerator
{
    public readonly JwtOptions _jwtOptions;
    public JwtGenerator(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }
    public string Generate(Guid id, string email)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new Claim[]
        {
            new (System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, id.ToString()),
            new (System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Email, email)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = credentials,
            Issuer = _jwtOptions.Issuer,
            Audience = _jwtOptions.Audience,
            Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiresInMinutes)
        };

        var handler = new JsonWebTokenHandler();
        return handler.CreateToken(tokenDescriptor);

    }
}

public interface IJwtGenerator
{
    string Generate(Guid id, string email);
}