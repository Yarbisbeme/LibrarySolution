using Azure;
using Library.Api.Controllers;
using Library.Application.DTOs;
using Library.Application.Interfaces;
using Library.Common.Dtos;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Library.Tests.Controllers
{
    public class LibrosController_Tests
    {
        private readonly Mock<IBookService> _mockService;
        private readonly LibrosController _controller;

        public LibrosController_Tests()
        {
            _mockService = new Mock<IBookService>();
            _controller = new LibrosController(_mockService.Object);
        }

        [Fact]
        public async Task ObtenerLibrosAntesDe2000_CuandoExisten_DeberiaRetornarOkConLibros()
        {
            // Arrange
            var librosMock = new List<LibroResponseDto>
            {
                new() { LibroId = 1, Titulo = "Don Quijote", AnioPublicacion = 1605 },
                new() { LibroId = 2, Titulo = "Cien años de soledad", AnioPublicacion = 1967 }
            };

            _mockService.Setup(s => s.ObtenerLibrosAntesDe2000Async())
                        .ReturnsAsync(librosMock);

            // Act
            var result = await _controller.ObtenerLibrosAntesDe2000();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<LibroResponseDto>>>(okResult.Value);

            Assert.True(response.Success);
            Assert.Equal(2, response.Data!.Count());
            Assert.All(response.Data!, l => Assert.True(l.AnioPublicacion < 2000));
        }

        // Caso de borde (sin libros antes del 2000)
        [Fact]
        public async Task ObtenerLibrosAntesDe2000_CuandoNoExisten_DeberiaRetornarListaVacia()
        {
            // Arrange
            _mockService.Setup(s => s.ObtenerLibrosAntesDe2000Async())
                        .ReturnsAsync(new List<LibroResponseDto>());

            // Act
            var result = await _controller.ObtenerLibrosAntesDe2000();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<LibroResponseDto>>>(okResult.Value);

            Assert.True(response.Success);
            Assert.Empty(response.Data!);
        }
    }
}
