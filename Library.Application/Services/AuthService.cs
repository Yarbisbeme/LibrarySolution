using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Library.Common.Dto;
using Library.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Library.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;

        // Mediante el constructor vamos a inyectar las dependecias que usaremos
        public AuthService(IConfiguration configuration, ILogger<AuthService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Implementacion de nuestro metodo para generar token
        /// <summary>
        private string GenerateToken(string username, string role)
        {
            var jwtSettings = _configuration.GetSection("jwt");
            var secret = jwtSettings["secret"] ?? throw new InvalidOperationException("No hay un secret configurado");
            var issuer = jwtSettings["issuer"];
            // Por si hacemos el front
            var audience = jwtSettings["audience"];

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Implementacion para nuestro servicio de autenticacion
        /// <summary>
        public async Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginDto dto)
        {
            try
            {
                await Task.Delay(500);

                string? role = null;
                string? username = null;

                if (
                    (dto.Username == "admin" && dto.Password == "admin123") ||
                    (dto.Username == "usuario" && dto.Password == "user123"))
                {
                    role = dto.Username;
                    username = dto.Username;
                }
                else
                {
                    _logger.LogWarning("Intento de login fallido para usuario: {Username}", dto.Username);
                    return ApiResponse<LoginResponseDto>.ErrorResponse(
                        "Credenciales inválidas",
                        new List<string> { "Usuario o clave incorrectas" }
                    );
                }

                var token = GenerateToken(username!, role!);

                return ApiResponse<LoginResponseDto>.SuccessResponse(
                    new LoginResponseDto
                    {
                        Username = username,
                        Role = role,
                        Token = token,
                        ExpiresAt = DateTime.UtcNow.AddHours(1)
                    },
                    "Login exitoso"
                );
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Error en configuración del JWT");
                return ApiResponse<LoginResponseDto>.ErrorResponse(
                    "Configuración JWT inválida",
                    new List<string> { ex.Message }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante el proceso de login para usuario: {Username}", dto.Username);
                return ApiResponse<LoginResponseDto>.ErrorResponse(
                    "Error interno del servidor",
                    new List<string> { "Ocurrió un error inesperado. Intente nuevamente más tarde." }
                );
            }
        }

    }
}