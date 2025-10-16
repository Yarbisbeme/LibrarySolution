using FluentAssertions;
using Library.Api.Controllers;
using Library.Application.Interfaces;
using Library.Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Library.Tests.Controllers
{
    public class LibrosControllerTests
    {
        private readonly Mock<IBookService> _mockBookService;
        private readonly LibrosController _controller;

        public LibrosControllerTests()
        {
            _mockBookService = new Mock<IBookService>();
            _controller = new LibrosController(_mockBookService.Object);
        }

        #region ObtenerLibrosAntesDe2000 Tests

        [Fact]
        public async Task ObtenerLibrosAntesDe2000_ReturnsOkResult_WithListOfBooks()
        {
            // Arrange
            var expectedBooks = new List<LibroResponseDto>
            {
                new LibroResponseDto { LibroId = 1, Titulo = "Cien años de soledad", AnioPublicacion = 1967 },
                new LibroResponseDto { LibroId = 2, Titulo = "1984", AnioPublicacion = 1949 }
            };

            _mockBookService
                .Setup(s => s.ObtenerLibrosAntesDe2000Async())
                .ReturnsAsync(expectedBooks);

            // Act
            var result = await _controller.ObtenerLibrosAntesDe2000();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var response = okResult.Value.Should().BeOfType<ApiResponse<IEnumerable<LibroResponseDto>>>().Subject;
            
            response.Success.Should().BeTrue();
            response.Data.Should().HaveCount(2);
            response.Data.All(b => b.AnioPublicacion < 2000).Should().BeTrue();
            
            _mockBookService.Verify(s => s.ObtenerLibrosAntesDe2000Async(), Times.Once);
        }

        [Fact]
        public async Task ObtenerLibrosAntesDe2000_ReturnsEmptyList_WhenNoBooksFound()
        {
            // Arrange
            _mockBookService
                .Setup(s => s.ObtenerLibrosAntesDe2000Async())
                .ReturnsAsync(new List<LibroResponseDto>());

            // Act
            var result = await _controller.ObtenerLibrosAntesDe2000();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var response = okResult.Value.Should().BeOfType<ApiResponse<IEnumerable<LibroResponseDto>>>().Subject;
            
            response.Success.Should().BeTrue();
            response.Data.Should().BeEmpty();
        }

        #endregion

        #region GetBooksByAuthor Tests

        [Fact]
        public async Task GetBooksByAuthor_ReturnsOkResult_WithBooksByAuthor()
        {
            // Arrange
            var autor = "Gabriel García Márquez";
            var expectedBooks = new List<LibroResponseDto>
            {
                new LibroResponseDto { LibroId = 1, Titulo = "Cien años de soledad", AnioPublicacion = 1967 },
                new LibroResponseDto { LibroId = 2, Titulo = "El amor en los tiempos del cólera", AnioPublicacion = 1985 }
            };

            _mockBookService
                .Setup(s => s.ObtenerLibrosPorAutorAsync(autor))
                .ReturnsAsync(expectedBooks);

            // Act
            var result = await _controller.GetBooksByAuthor(autor);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var response = okResult.Value.Should().BeOfType<ApiResponse<IEnumerable<LibroResponseDto>>>().Subject;
            
            response.Success.Should().BeTrue();
            response.Data.Should().HaveCount(2);
            response.Message.Should().Be("Libro encontrado");
            
            _mockBookService.Verify(s => s.ObtenerLibrosPorAutorAsync(autor), Times.Once);
        }

        [Fact]
        public async Task GetBooksByAuthor_ReturnsEmptyList_WhenAuthorNotFound()
        {
            // Arrange
            var autor = "Autor Inexistente";
            _mockBookService
                .Setup(s => s.ObtenerLibrosPorAutorAsync(autor))
                .ReturnsAsync(new List<LibroResponseDto>());

            // Act
            var result = await _controller.GetBooksByAuthor(autor);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var response = okResult.Value.Should().BeOfType<ApiResponse<IEnumerable<LibroResponseDto>>>().Subject;
            
            response.Data.Should().BeEmpty();
        }

        #endregion

        #region GetListBooksByTitle Tests

        [Fact]
        public async Task GetListBooksByTitle_ReturnsOkResult_WithBooks()
        {
            // Arrange
            var title = "Cien años";
            var expectedBooks = new List<LibroResponseDto>
            {
                new LibroResponseDto { LibroId = 1, Titulo = "Cien años de soledad", AnioPublicacion = 1967 }
            };

            _mockBookService
                .Setup(s => s.ObtenerLibroPorTituloAsync(title))
                .ReturnsAsync(expectedBooks);

            // Act
            var result = await _controller.GetListBooksByTitle(title);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var response = okResult.Value.Should().BeOfType<ApiResponse<IEnumerable<LibroResponseDto>>>().Subject;
            
            response.Success.Should().BeTrue();
            response.Data.Should().HaveCount(1);
            response.Message.Should().Be("Libros encontrados");
            
            _mockBookService.Verify(s => s.ObtenerLibroPorTituloAsync(title), Times.Once);
        }

        #endregion

        #region CrearLibro Tests

        [Fact]
        public async Task CrearLibro_ReturnsCreatedAtAction_WithNewBook()
        {
            // Arrange
            var libroDto = new LibroDto
            {
                Titulo = "Don Quijote",
                AnioPublicacion = 1605,
                Genero = "Novela"
            };

            var createdBook = new LibroResponseDto
            {
                LibroId = 1,
                Titulo = "Don Quijote",
                AnioPublicacion = 1605,
                Genero = "Novela"
            };

            _mockBookService
                .Setup(s => s.CrearLibroAsync(libroDto))
                .ReturnsAsync(createdBook);

            // Act
            var result = await _controller.CrearLibro(libroDto);

            // Assert
            var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdResult.ActionName.Should().Be(nameof(LibrosController.ObtenerLibrosAntesDe2000));
            
            var response = createdResult.Value.Should().BeOfType<ApiResponse<LibroResponseDto>>().Subject;
            response.Success.Should().BeTrue();
            response.Data.Should().NotBeNull();
            response.Data.LibroId.Should().Be(1);
            response.Message.Should().Be("Libro creado exitosamente");
            
            _mockBookService.Verify(s => s.CrearLibroAsync(libroDto), Times.Once);
        }

        [Fact]
        public async Task CrearLibro_ThrowsException_WhenModelStateInvalid()
        {
            // Arrange
            var libroDto = new LibroDto();
            _controller.ModelState.AddModelError("Titulo", "El título es requerido");

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await _controller.CrearLibro(libroDto)
            );
            
            _mockBookService.Verify(s => s.CrearLibroAsync(It.IsAny<LibroDto>()), Times.Never);
        }

        #endregion

        #region DeleteBook Tests

        [Fact]
        public async Task DeleteBook_ReturnsOkResult_WithTrue_WhenDeleteSuccessful()
        {
            // Arrange
            var bookId = 1;
            _mockBookService
                .Setup(s => s.EliminarLibroAsync(bookId))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteBook(bookId);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var response = okResult.Value.Should().BeOfType<ApiResponse<bool>>().Subject;
            
            response.Success.Should().BeTrue();
            response.Data.Should().BeTrue();
            response.Message.Should().Be("Se ha eliminado el libro");
            
            _mockBookService.Verify(s => s.EliminarLibroAsync(bookId), Times.Once);
        }

        [Fact]
        public async Task DeleteBook_ReturnsOkResult_WithFalse_WhenBookNotFound()
        {
            // Arrange
            var bookId = 999;
            _mockBookService
                .Setup(s => s.EliminarLibroAsync(bookId))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteBook(bookId);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var response = okResult.Value.Should().BeOfType<ApiResponse<bool>>().Subject;
            
            response.Data.Should().BeFalse();
        }

        #endregion

        #region UpdateBook Tests

        [Fact]
        public async Task UpdateBook_ReturnsOkResult_WithUpdatedBook()
        {
            // Arrange
            var bookId = 1;
            var libroDto = new LibroDto
            {
                Titulo = "Don Quijote - Edición Actualizada",
                AnioPublicacion = 1605,
                Genero = "Novela Clásica"
            };

            var updatedBook = new LibroResponseDto
            {
                LibroId = bookId,
                Titulo = "Don Quijote - Edición Actualizada",
                AnioPublicacion = 1605,
                Genero = "Novela Clásica"
            };

            _mockBookService
                .Setup(s => s.ActualizarLibroAsync(bookId, It.IsAny<LibroDto>()))
                .ReturnsAsync(updatedBook);

            // Act
            var result = await _controller.UpdateBook(bookId, libroDto);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var response = okResult.Value.Should().BeOfType<ApiResponse<LibroResponseDto>>().Subject;
            
            response.Success.Should().BeTrue();
            response.Data.Should().NotBeNull();
            response.Data.LibroId.Should().Be(bookId);
            response.Message.Should().Be("Se ha actualizado correctamente el libro");
            
            _mockBookService.Verify(s => s.ActualizarLibroAsync(bookId, It.IsAny<LibroDto>()), Times.Once);
        }

        [Fact]
        public async Task UpdateBook_MapsPropertiesCorrectly()
        {
            // Arrange
            var bookId = 1;
            var libroDto = new LibroDto
            {
                Titulo = "Nuevo Título",
                AnioPublicacion = 2020,
                Genero = "Ficción"
            };

            LibroDto capturedDto = null!;
            _mockBookService
                .Setup(s => s.ActualizarLibroAsync(bookId, It.IsAny<LibroDto>()))
                .Callback<int, LibroDto>((id, dto) => capturedDto = dto)
                .ReturnsAsync(new LibroResponseDto());

            // Act
            await _controller.UpdateBook(bookId, libroDto);

            // Assert
            capturedDto.Should().NotBeNull();
            capturedDto.Titulo.Should().Be("Nuevo Título");
            capturedDto.AnioPublicacion.Should().Be(2020);
            capturedDto.Genero.Should().Be("Ficción");
        }

        #endregion
    }
}