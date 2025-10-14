
using Library.Api.Controllers;
using Library.Application.Interfaces;
using Library.Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Library.Tests.Controllers
{
    public class PrestamosTests
    {
        private readonly Mock<ILoanService> _mockService;
        private readonly PrestamosController _controller;

        public PrestamosTests()
        {
            _mockService = new Mock<ILoanService>();
            _controller = new PrestamosController(_mockService.Object);
        }

        #region Get/API/Prestamos

        [Fact]
        public async Task CrearPrestamo_CuandoExitoso_DeberiaRetornarOk()
        {
            // Arrange
            var dto = new PostPrestamoDto { BookId = 1, Devolucion_Prestamo = DateTime.UtcNow.AddDays(7) };
            var response = new LoanResponse
            {
                LibroId = 1,
                Titulo = "El Principito",
                Autor = "Antoine de Saint-Exupéry",
                Fecha_Prestamo = DateTime.UtcNow,
                Fecha_Devolucion = dto.Devolucion_Prestamo
            };

            _mockService.Setup(s => s.PostLoan(dto)).ReturnsAsync(response);

            // Act
            var result = await _controller.CrearPrestamo(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<LoanResponse>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal("El Principito", apiResponse.Data?.Titulo);
        }

        [Fact]
        public async Task CrearPrestamo_CuandoLibroNoExiste_DeberiaLanzarKeyNotFound()
        {
            // Arrange
            var dto = new PostPrestamoDto { BookId = 999 };
            _mockService.Setup(s => s.PostLoan(dto))
                        .ThrowsAsync(new KeyNotFoundException("No se encontró el libro"));

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _controller.CrearPrestamo(dto));
        }

        [Fact]
        public async Task CrearPrestamo_CuandoExcepcion_DeberiaLanzarExcepcion()
        {
            // Arrange
            var dto = new PostPrestamoDto { BookId = 1 };
            _mockService.Setup(s => s.PostLoan(dto))
                        .ThrowsAsync(new Exception("Error interno"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _controller.CrearPrestamo(dto));
        }

        #endregion

        #region  GET /api/prestamos/no-devueltos

        [Fact]
        public async Task ObtenerNoDevueltos_CuandoExisten_DeberiaRetornarOk()
        {
            // Arrange
            var prestamos = new List<PrestamoNoDevueltoDto>
            {
                new() { PrestamoId = 1, LibroId = 1, Titulo = "1984", AutorId = 1, Nombre = "George Orwell" },
                new() { PrestamoId = 2, LibroId = 2, Titulo = "Fahrenheit 451", AutorId = 2, Nombre = "Ray Bradbury" }
            };

            _mockService.Setup(s => s.ObtenerPrestamosNoDevueltosAsync())
                        .ReturnsAsync(prestamos);

            // Act
            var result = await _controller.ObtenerNoDevueltos();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<IEnumerable<PrestamoNoDevueltoDto>>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(2, apiResponse.Data!.Count());
        }

        [Fact]
        public async Task ObtenerNoDevueltos_CuandoExcepcion_DeberiaLanzarExcepcion()
        {
            // Arrange
            _mockService.Setup(s => s.ObtenerPrestamosNoDevueltosAsync())
                        .ThrowsAsync(new Exception("Error DB"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _controller.ObtenerNoDevueltos());
        }
        #endregion

        #region PUT /api/prestamos/{id}

        [Fact]
        public async Task ActualizarDevolucion_CuandoExitoso_DeberiaRetornarOk()
        {
            // Arrange
            var dto = new ActualizarDevolucionDto { FechaDevolucion = DateTime.UtcNow };
            _mockService.Setup(s => s.UpdateReturnDateAsync(1, dto.FechaDevolucion))
                        .ReturnsAsync(dto);

            // Act
            var result = await _controller.ActualizarDevolucion(1, dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<ActualizarDevolucionDto>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal(dto.FechaDevolucion, apiResponse.Data?.FechaDevolucion);
        }

        [Fact]
        public async Task ActualizarDevolucion_CuandoNoExiste_DeberiaLanzarKeyNotFound()
        {
            // Arrange
            var dto = new ActualizarDevolucionDto { FechaDevolucion = DateTime.UtcNow };
            _mockService.Setup(s => s.UpdateReturnDateAsync(999, dto.FechaDevolucion))
                        .ThrowsAsync(new KeyNotFoundException("No encontrado"));

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _controller.ActualizarDevolucion(999, dto));
        }

        #endregion

        #region DELETE /api/prestamos/{id}

        [Fact]
        public async Task EliminarPrestamo_CuandoExitoso_DeberiaRetornarOk()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteLoanAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.EliminarPrestamo(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<object>>(okResult.Value);
            Assert.True(apiResponse.Success);
        }

        [Fact]
        public async Task EliminarPrestamo_CuandoNoExiste_DeberiaLanzarKeyNotFound()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteLoanAsync(999))
                        .ThrowsAsync(new KeyNotFoundException("No se encontró el préstamo"));

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _controller.EliminarPrestamo(999));
        }

        #endregion

    }
}