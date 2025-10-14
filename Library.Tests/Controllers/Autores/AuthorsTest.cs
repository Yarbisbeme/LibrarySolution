
using FluentAssertions;
using Library.Api.Controllers;
using Library.Application.Interfaces;
using Library.Common.Dto;
using Library.Common.Dto.Autores;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Library.Api.Tests.Controllers
{
    public class AuthorControllerTests
    {
        private readonly Mock<IAuthorService> _authorServiceMock;
        private readonly AuthorController _controller;

        public AuthorControllerTests()
        {
            _authorServiceMock = new Mock<IAuthorService>();
            _controller = new AuthorController(_authorServiceMock.Object);
        }

        // 🧩 Prueba GetAuthors
        [Fact]
        public async Task GetAuthors_ShouldReturnOk_WithListOfAuthors()
        {
            // Arrange
            var authors = new List<AuthorResponse>
            {
                new AuthorResponse { Nombre = "Gabriel García Márquez", Nacionalidad = "Colombiano" }
            };

            _authorServiceMock
                .Setup(s => s.GetAllAuthorsAsync())
                .ReturnsAsync(ApiResponse<List<AuthorResponse>>.SuccessResponse(authors, "Autores obtenidos correctamente."));

            // Act
            var result = await _controller.GetAuthors();

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);

            var response = okResult.Value as ApiResponse<List<AuthorResponse>>;
            response.Should().NotBeNull();
            response!.Success.Should().BeTrue();
            response.Data.Should().HaveCount(1);
        }

        // 🧩 Prueba GetAuthorById
        [Fact]
        public async Task GetAuthorById_ShouldReturnOk_WhenAuthorExists()
        {
            // Arrange
            var author = new AuthorResponse { Nombre = "Isabel Allende", Nacionalidad = "Chilena" };
            _authorServiceMock
                .Setup(s => s.GetAuthorByIdAsync(1))
                .ReturnsAsync(ApiResponse<AuthorResponse>.SuccessResponse(author, "Autor encontrado."));

            // Act
            var result = await _controller.GetAuthorById(1);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);

            var response = okResult.Value as ApiResponse<AuthorResponse>;
            response!.Success.Should().BeTrue();
            response.Data!.Nombre.Should().Be("Isabel Allende");
        }

        // 🧩 Prueba CreateAuthor
        [Fact]
        public async Task CreateAuthor_ShouldReturnCreated_WhenAuthorIsCreated()
        {
            // Arrange
            var authorDto = new AuthorDto { Nombre = "Jorge Luis Borges", Nacionalidad = "Argentino" };
            var createdAuthor = new CreateAuthorDto { AuthorId = 1, Nombre = authorDto.Nombre, Nacionalidad = authorDto.Nacionalidad };

            _authorServiceMock
                .Setup(s => s.CreateAuthorAsync(authorDto))
                .ReturnsAsync(ApiResponse<CreateAuthorDto>.SuccessResponse(createdAuthor, "Autor creado correctamente."));

            // Act
            var result = await _controller.CreateAuthor(authorDto);

            // Assert
            var createdResult = result as CreatedAtActionResult;
            createdResult.Should().NotBeNull();
            createdResult!.ActionName.Should().Be(nameof(AuthorController.GetAuthorById));

            var response = createdResult.Value as ApiResponse<CreateAuthorDto>;
            response!.Data!.AuthorId.Should().Be(1);
            response.Success.Should().BeTrue();
        }

        // 🧩 Prueba UpdateAuthor
        [Fact]
        public async Task UpdateAuthor_ShouldReturnOk_WhenAuthorUpdated()
        {
            // Arrange
            var authorDto = new AuthorDto { Nombre = "Julio Cortázar", Nacionalidad = "Argentino" };

            _authorServiceMock
                .Setup(s => s.UpdateAuthorAsync(1, authorDto))
                .ReturnsAsync(ApiResponse<AuthorDto>.SuccessResponse(authorDto, "Autor actualizado correctamente."));

            // Act
            var result = await _controller.UpdateAuthor(1, authorDto);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);

            var response = okResult.Value as ApiResponse<AuthorDto>;
            response!.Data!.Nombre.Should().Be("Julio Cortázar");
            response.Success.Should().BeTrue();
        }

        // 🧩 Prueba DeleteAuthor
        [Fact]
        public async Task DeleteAuthor_ShouldReturnOk_WhenAuthorDeleted()
        {
            // Arrange
            _authorServiceMock
                .Setup(s => s.DeleteAuthorAsync(1))
                .ReturnsAsync(ApiResponse<bool>.SuccessResponse(true, "Autor eliminado correctamente."));

            // Act
            var result = await _controller.DeleteAuthor(1);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);

            var response = okResult.Value as ApiResponse<bool>;
            response!.Data.Should().BeTrue();
            response.Success.Should().BeTrue();
        }
    }
}
