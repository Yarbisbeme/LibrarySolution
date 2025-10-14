using System.Threading.Tasks;
using FluentAssertions;
using Library.Api.Controllers;
using Library.Application.Interfaces;
using Library.Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Library.Api.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IAuthService> _authServiceMock;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _authServiceMock = new Mock<IAuthService>();
            _controller = new AuthController(_authServiceMock.Object);
        }

        // 🧩 Prueba: Login exitoso
        [Fact]
        public async Task Login_ShouldReturnOk_WhenCredentialsAreValid()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Username = "admin",
                Password = "12345"
            };

            var loginResponse = new LoginResponseDto
            {
                Token = "fake-jwt-token",
                Username = "admin",
                Role = "admin"
            };

            _authServiceMock
                .Setup(s => s.LoginAsync(loginDto))
                .ReturnsAsync(ApiResponse<LoginResponseDto>.SuccessResponse(loginResponse, "Inicio de sesión exitoso."));

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);

            var response = okResult.Value as ApiResponse<LoginResponseDto>;
            response.Should().NotBeNull();
            response!.Success.Should().BeTrue();
            response.Data!.Token.Should().Be("fake-jwt-token");
            response.Message.Should().Be("Inicio de sesión exitoso.");
        }

        // 🧩 Prueba: Login fallido (credenciales incorrectas)
        [Fact]
        public async Task Login_ShouldReturnBadRequest_WhenCredentialsAreInvalid()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Username = "user",
                Password = "wrongpassword"
            };

            _authServiceMock
                .Setup(s => s.LoginAsync(loginDto))
                .ReturnsAsync(ApiResponse<LoginResponseDto>.ErrorResponse("Credenciales inválidas."));

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult!.StatusCode.Should().Be(400);

            var response = badRequestResult.Value as ApiResponse<LoginResponseDto>;
            response.Should().NotBeNull();
            response!.Success.Should().BeFalse();
            response.Message.Should().Be("Credenciales inválidas.");
        }

        // 🧩 Prueba: Excepción inesperada
        [Fact]
        public async Task Login_ShouldThrowException_WhenServiceFails()
        {
            // Arrange
            var loginDto = new LoginDto { Username = "admin", Password = "12345" };

            _authServiceMock
                .Setup(s => s.LoginAsync(loginDto))
                .ThrowsAsync(new System.Exception("Error inesperado en el servicio."));

            // Act
            var act = async () => await _controller.Login(loginDto);

            // Assert
            await act.Should().ThrowAsync<System.Exception>()
                .WithMessage("Error inesperado en el servicio.");
        }
    }
}
