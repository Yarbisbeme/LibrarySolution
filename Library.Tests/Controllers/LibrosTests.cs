using Library.Application.Interfaces;
using Library.Common.Dto;
using Library.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Library.Tests.Controllers{
    public class LibrosControllerTests
    {
        /// <Summary>
        /// Pruebas para crear los libros
        /// <Summary>
        [Fact]
        public async Task CrearLibro_RetornaCreated_WhenLibroCreado()
        {
            // Arrange
            var mockService = new Mock<IBookService>();
            var dto = new LibroDto { Titulo = "El Principito", AnioPublicacion = 1943, AutorId = 1, Genero = "Fábula" };
            var responseDto = new LibroResponseDto { LibroId = 1, Titulo = dto.Titulo, AnioPublicacion = dto.AnioPublicacion, Autor = "Antoine de Saint-Exupéry", Genero = dto.Genero };

            mockService.Setup(s => s.CrearLibroAsync(dto)).ReturnsAsync(responseDto);

            var controller = new LibrosController(mockService.Object);

            // Act
            var result = await controller.CrearLibro(dto) as CreatedAtActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode);
            var apiResponse = result.Value as dynamic;
            Assert.Equal(dto.Titulo, apiResponse!.Data.Titulo);
        }

        [Fact]
        public async Task CrearLibro_CuandoAutorNoExiste_RetornaNotFound()
        {
            // Arrange
            var mockService = new Mock<IBookService>();
            var dto = new LibroDto { Titulo = "Libro Fantasma", AnioPublicacion = 2020, AutorId = 999, Genero = "Misterio" };

            // Configuramos que el servicio lance una excepción
            mockService.Setup(s => s.CrearLibroAsync(dto)).ThrowsAsync(new KeyNotFoundException("No se encontró el autor"));

            var controller = new LibrosController(mockService.Object);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<KeyNotFoundException>(() => controller.CrearLibro(dto));
            Assert.Equal("No se encontró el autor", ex.Message);
        }

        [Fact]
        public async Task CrearLibro_CuandoOcurreErrorInterno_RetornaInternalServerError()
        {
            // Arrange
            var mockService = new Mock<IBookService>();
            var dto = new LibroDto { Titulo = "ErrorLibro", AnioPublicacion = 2023, AutorId = 1, Genero = "Drama" };

            // Simulamos un error inesperado en el servicio
            mockService.Setup(s => s.CrearLibroAsync(dto)).ThrowsAsync(new InvalidOperationException("Error al guardar el libro"));

            var controller = new LibrosController(mockService.Object);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => controller.CrearLibro(dto));
            Assert.Equal("Error al guardar el libro", ex.Message);
        }


        /// <Summary>
        /// Pruebas para obtener los libros
        /// <Summary>
        [Fact]
        public async Task ObtenerLibrosAntesDe2000_RetornaOkConLista()
        {
            // Arrange
            var mockService = new Mock<IBookService>();
            var libros = new List<LibroResponseDto>
            {
                new LibroResponseDto { LibroId = 1, Titulo = "1984", AnioPublicacion = 1949, Autor = "George Orwell", Genero = "Distopía" }
            };

            mockService.Setup(s => s.ObtenerLibrosAntesDe2000Async()).ReturnsAsync(libros);
            var controller = new LibrosController(mockService.Object);

            // Act
            var result = await controller.ObtenerLibrosAntesDe2000() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            var apiResponse = result.Value as dynamic;
            Assert.Single(apiResponse!.Data);
        }

        [Fact]
        public async Task ObtenerLibrosAntesDe2000_CuandoOcurreErrorInterno_LanzaExcepcion()
        {
            // Arrange
            var mockService = new Mock<IBookService>();
            mockService.Setup(s => s.ObtenerLibrosAntesDe2000Async()).ThrowsAsync(new Exception("Error al consultar libros"));

            var controller = new LibrosController(mockService.Object);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => controller.ObtenerLibrosAntesDe2000());
            Assert.Equal("Error al consultar libros", ex.Message);
        }

    }
}