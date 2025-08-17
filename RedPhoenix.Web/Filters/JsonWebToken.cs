namespace RedPhoenix.Web.Filters;


using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

using Data.ViewModels;

public class JsonWebToken(IConfiguration config)
{
    private readonly IConfiguration _config = config
        ?? throw new ArgumentNullException(nameof(config));

    public string GenerateJwtToken(UserViewModel userViewModel)
    {
        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"] ?? string.Empty));
        var credentials = new SigningCredentials(securityKey,
            SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim("Id", userViewModel.Id),
            new Claim("name", userViewModel.Code),
            new Claim(ClaimTypes.NameIdentifier, userViewModel.Name),
            new Claim(ClaimTypes.Role, userViewModel.RoleCode),
            new Claim(ClaimTypes.SerialNumber, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            signingCredentials: credentials,
            expires: DateTime.MaxValue
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

