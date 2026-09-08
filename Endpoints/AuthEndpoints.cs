using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace ExpenseTrackerApi;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var auth = app.MapGroup("/auth");
        auth.MapPost("/register", Register);
        auth.MapPost("/login", Login);
    }

    public static async Task<IResult> Register(AuthDTO dto, UserManager<IdentityUser> userManager)
    {
        var user = new IdentityUser
        {
            UserName = dto.Email,
            Email = dto.Email,
        };
        var result = await userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            return TypedResults.BadRequest(result.Errors);
        
        return TypedResults.Ok(new { Message = "User created successfully", UserId = user.Id });
    }

    public static async Task<IResult> Login(AuthDTO dto, UserManager<IdentityUser> userManager, IConfiguration configuration)
    {
        var user = await userManager.FindByNameAsync(dto.Email);
        if (user is null || !await userManager.CheckPasswordAsync(user, dto.Password))
        {
            return TypedResults.Unauthorized();
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email ?? throw new InvalidOperationException()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var jwtSettings = configuration.GetSection("Jwt");
        var key = Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? throw new InvalidOperationException("JWT key not configured"));
        var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        ); 
        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        
        return TypedResults.Ok(new { Token = tokenString });
    }
}
