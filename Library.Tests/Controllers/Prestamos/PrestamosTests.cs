using Library.Api.Controllers;
using Library.Application.DTOs;
using Library.Application.Interfaces;
using Library.Common.Dtos;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Library.Tests.Controllers
{
    public class PrestamosControllerTests
    {
        private readonly Mock<ILoanService> _mockService;
        private readonly PrestamosController _controller;

        public PrestamosControllerTests()
        {
            _mockService = new Mock<ILoanService>();
            _controller = new PrestamosController(_mockService.Object);
        }


        // GET /api/prestamos/no-devueltos
        // =========================================================
        [Fact]
        public async Task ObtenerNoDevueltos_CuandoExisten_DeberiaRetornarOkConLista()
        {
            // Arrange
            var prestamosMock = new List<PrestamoNoDevueltoDto>
            {
                new() { AutorId = 1, Nombre = "Gabriel García Márquez", LibroId = 1, Titulo = "Cien años de soledad" },
                new() { AutorId = 2, Nombre = "Isabel Allende", LibroId = 2, Titulo = "La casa de los espíritus" }
            };

            _mockService.Setup(s => s.ObtenerPrestamosNoDevueltosAsync())
                        .ReturnsAsync(prestamosMock);

            // Act
            var result = await _controller.ObtenerNoDevueltos();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<PrestamoNoDevueltoDto>>>(okResult.Value);

            Assert.True(response.Success);
            Assert.Equal(2, response.Data?.Count());
        }

        [Fact]
        public async Task ObtenerNoDevueltos_CuandoNoExisten_DeberiaRetornarListaVacia()
        {
            // Arrange
            _mockService.Setup(s => s.ObtenerPrestamosNoDevueltosAsync())
                        .ReturnsAsync(new List<PrestamoNoDevueltoDto>());

            // Act
            var result = await _controller.ObtenerNoDevueltos();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<PrestamoNoDevueltoDto>>>(okResult.Value);

            Assert.True(response.Success);
            Assert.Empty(response.Data ?? Enumerable.Empty<PrestamoNoDevueltoDto>());
        }

        [Fact]
        public async Task ObtenerNoDevueltos_CuandoLanzaExcepcion_DeberiaRetornar500()
        {
            // Arrange
            _mockService.Setup(s => s.ObtenerPrestamosNoDevueltosAsync())
                        .ThrowsAsync(new Exception("DB error"));

            // Act
            var result = await _controller.ObtenerNoDevueltos();

            // Assert
            var objResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objResult.StatusCode);
        }


        // PUT /api/prestamos/{id}
        // =========================================================
        [Fact]
        public async Task ActualizarDevolucion_CuandoExiste_DeberiaRetornarOk()
        {
            // Arrange
            var dto = new ActualizarDevolucionDto { FechaDevolucion = DateTime.Now };
            _mockService.Setup(s => s.UpdateReturnDateAsync(1, It.IsAny<DateTime>()))
                        .ReturnsAsync(dto);

            // Act
            var result = await _controller.ActualizarDevolucion(1, dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<object>>(okResult.Value);

            Assert.True(response.Success);
            Assert.Contains("actualizado", response.Message.ToLower());
        }

        [Fact]
        public async Task ActualizarDevolucion_CuandoNoExiste_DeberiaRetornar404()
        {
            // Arrange
            var dto = new ActualizarDevolucionDto { FechaDevolucion = DateTime.UtcNow };
            _mockService.Setup(s => s.UpdateReturnDateAsync(999, dto.FechaDevolucion))
                        .ThrowsAsync(new KeyNotFoundException());

            // Act
            var result = await _controller.ActualizarDevolucion(999, dto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Contains("no encontrado", notFoundResult.Value!.ToString()!.ToLower());
        }

        [Fact]
        public async Task ActualizarDevolucion_CuandoExcepcion_DeberiaRetornar500()
        {
            // Arrange
            var dto = new ActualizarDevolucionDto { FechaDevolucion = DateTime.UtcNow };
            _mockService.Setup(s => s.UpdateReturnDateAsync(It.IsAny<int>(), It.IsAny<DateTime>()))
                        .ThrowsAsync(new Exception("Fallo interno"));

            // Act
            var result = await _controller.ActualizarDevolucion(1, dto);

            // Assert
            var objResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objResult.StatusCode);
        }


        // DELETE /api/prestamos/{id}
        // =========================================================
        [Fact]
        public async Task EliminarPrestamo_CuandoExiste_DeberiaRetornarOk()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteLoanAsync(1))
                        .ReturnsAsync(true);

            // Act
            var result = await _controller.EliminarPrestamo(1);

            // Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Contains("eliminado", okResult.Value!.ToString()!.ToLower());
        }

        [Fact]
        public async Task EliminarPrestamo_CuandoNoExiste_DeberiaRetornar404()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteLoanAsync(999))
                        .ThrowsAsync(new KeyNotFoundException());

            // Act
            var result = await _controller.EliminarPrestamo(999);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Contains("no encontrado", notFound.Value!.ToString()!.ToLower());
        }

        [Fact]
        public async Task EliminarPrestamo_CuandoExcepcion_DeberiaRetornar500()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteLoanAsync(It.IsAny<int>()))
                        .ThrowsAsync(new Exception("DB crash"));

            // Act
            var result = await _controller.EliminarPrestamo(1);

            // Assert
            var objResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objResult.StatusCode);
        }
    }
}
