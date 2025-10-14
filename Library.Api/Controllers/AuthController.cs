using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Library.Application.DTOs;
using Library.Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Library.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IConfiguration configuration, ILogger<AuthController> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<LoginResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public ActionResult<ApiResponse<LoginResponseDto>> Login([FromBody] LoginDto loginDto)
    {
        try
        {
            // IMPORTANTE: Esto es solo para demostración/pruebas
            // En producción, validar contra una base de datos de usuarios con passwords hasheados

            string? role = null;
            string? username = null;

            // Credenciales hardcodeadas solo para la prueba técnica
            if (loginDto.Username == "admin" && loginDto.Password == "admin123")
            {
                role = "admin";
                username = "admin";
            }
            else if (loginDto.Username == "usuario" && loginDto.Password == "usuario123")
            {
                role = "usuario";
                username = "usuario";
            }
            else
            {
                _logger.LogWarning("Intento de login fallido para usuario: {Username}", loginDto.Username);
                return BadRequest(ApiResponse<object>.ErrorResponse(
                    "Credenciales inválidas",
                    new List<string> { "Usuario o contraseña incorrectos" }
                ));
            }

            var token = GenerarToken(username, role);

            _logger.LogInformation("Login exitoso para usuario: {Username}, Rol: {Role}", username, role);

            return Ok(ApiResponse<LoginResponseDto>.SuccessResponse(
                new LoginResponseDto
                {
                    Token = token,
                    Username = username,
                    Role = role,
                    ExpiresAt = DateTime.UtcNow.AddHours(8)
                },
                "Login exitoso"
            ));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al generar token");
            return StatusCode(500, ApiResponse<object>.ErrorResponse(
                "Error al procesar login",
                new List<string> { "Ocurrió un error inesperado. Intente nuevamente." }
            ));
        }
    }

    private string GenerarToken(string username, string role)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var secretKey = jwtSettings["Secret"] ?? throw new InvalidOperationException("JWT Secret no configurado");
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    [HttpGet("credenciales-prueba")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    public ActionResult<ApiResponse<object>> ObtenerCredencialesPrueba()
    {
        var credenciales = new
        {
            Mensaje = "Credenciales para pruebas (SOLO DESARROLLO)",
            Admin = new { Username = "admin", Password = "admin123", Rol = "admin" },
            Usuario = new { Username = "usuario", Password = "usuario123", Rol = "usuario" },
            Nota = "Las credenciales de admin permiten acceder a todos los endpoints protegidos"
        };

        return Ok(ApiResponse<object>.SuccessResponse(credenciales, "Credenciales de prueba"));
    }
}


