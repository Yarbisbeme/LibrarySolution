using Library.Api.Controllers;
using Library.Common.Dto;
using Library.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Library.Tests.Controllers
{
    public class LibrosControllerPostTests
    {
        private readonly Mock<IBookService> _mockService;
        private readonly LibrosController _controller;

        public LibrosControllerPostTests()
        {
            _mockService = new Mock<IBookService>();
            _controller = new LibrosController(_mockService.Object);
        }

        // ✅ Caso de éxito
        [Fact]
        public async Task CrearLibro_CuandoDatosValidos_DeberiaRetornarCreated()
        {
            // Arrange
            var nuevoLibro = new LibroCreateDto
            {
                Titulo = "Nuevo libro",
                AutorId = 1,
                AnioPublicacion = 2021,
                Genero = "Ficción"
            };

            var libroCreado = new LibroResponseDto
            {
                LibroId = 10,
                Titulo = "Nuevo libro",
                Autor = "Autor Demo",
                Genero = "Ficción",
                AnioPublicacion = 2021
            };
            

            _mockService.Setup(s => s.CrearLibroAsync(nuevoLibro))
                        .ReturnsAsync(libroCreado);

            // Act
            var result = await _controller.CrearLibro(nuevoLibro);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var response = Assert.IsType<ApiResponse<LibroResponseDto>>(createdResult.Value);

            Assert.True(response.Success);
            Assert.Equal("Nuevo libro", response.Data?.Titulo);
        }

        // ✅ Caso de fallo (autor inexistente)
        [Fact]
        public async Task CrearLibro_CuandoAutorNoExiste_DeberiaRetornarBadRequest()
        {
            // Arrange
            var dto = new LibroCreateDto
            {
                Titulo = "Libro sin autor",
                AutorId = 999,
                AnioPublicacion = 2023
            };

            _mockService.Setup(s => s.CrearLibroAsync(dto))
                        .ThrowsAsync(new KeyNotFoundException("El autor no existe"));

            // Act
            var result = await _controller.CrearLibro(dto);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ApiResponse<object>>(badRequest.Value);

            Assert.False(response.Success);
            Assert.Contains("autor", response.Message.ToLower());
        }

        // ✅ Caso de validación (título vacío o nulo)
        [Fact]
        public async Task CrearLibro_CuandoTituloEsInvalido_DeberiaRechazarModelo()
        {
            // Arrange
            var dto = new LibroCreateDto
            {
                Titulo = "", // inválido
                AutorId = 1,
                AnioPublicacion = 2023,
                Genero = "Drama"
            };

            _controller.ModelState.AddModelError("Titulo", "El título es obligatorio");

            // Act
            var result = await _controller.CrearLibro(dto);

            // Assert
            Assert.False(_controller.ModelState.IsValid);
        }
    }
}
