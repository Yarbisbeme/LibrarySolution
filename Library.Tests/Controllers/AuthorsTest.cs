using FluentAssertions;
using Library.Api.Controllers;
using Library.Application.Interfaces;
using Library.Common.Dto;
using Library.Common.Dto.Autores;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Library.Tests.Controllers
{
    public class AuthorControllerTests
    {
        private readonly Mock<IAuthorService> _mockAuthorService;
        private readonly AuthorController _controller;

        public AuthorControllerTests()
        {
            _mockAuthorService = new Mock<IAuthorService>();
            _controller = new AuthorController(_mockAuthorService.Object);
        }

        #region GetAuthors Tests

        [Fact]
        public async Task GetAuthors_ReturnsOkResult_WithListOfAuthors()
        {
            // Arrange
            var expectedAuthors = new List<AuthorResponse>
            {
                new AuthorResponse { Autor_id = 1, Nombre = "Gabriel García Márquez" },
                new AuthorResponse { Autor_id = 2, Nombre = "Isabel Allende" }
            };

            _mockAuthorService
                .Setup(s => s.GetAllAuthorsAsync())
                .ReturnsAsync(expectedAuthors);

            // Act
            var result = await _controller.GetAuthors();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var response = okResult.Value.Should().BeOfType<ApiResponse<IEnumerable<AuthorResponse>>>().Subject;
            
            response.Success.Should().BeTrue();
            response.Data.Should().HaveCount(2);
            response.Message.Should().Be("Se han obtenido satisfactoriamente todos los autores");
            
            _mockAuthorService.Verify(s => s.GetAllAuthorsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAuthors_ReturnsOkResult_WithEmptyList_WhenNoAuthors()
        {
            // Arrange
            _mockAuthorService
                .Setup(s => s.GetAllAuthorsAsync())
                .ReturnsAsync(new List<AuthorResponse>());

            // Act
            var result = await _controller.GetAuthors();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var response = okResult.Value.Should().BeOfType<ApiResponse<IEnumerable<AuthorResponse>>>().Subject;
            
            response.Success.Should().BeTrue();
            response.Data.Should().BeEmpty();
        }

        #endregion

        #region GetAuthorById Tests

        [Fact]
        public async Task GetAuthorById_ReturnsOkResult_WithAuthor_WhenAuthorExists()
        {
            // Arrange
            var authorId = 1;
            var expectedAuthor = new AuthorResponse 
            { 
                Autor_id = authorId, 
                Nombre = "Gabriel García Márquez" 
            };

            _mockAuthorService
                .Setup(s => s.GetAuthorByIdAsync(authorId))
                .ReturnsAsync(expectedAuthor);

            // Act
            var result = await _controller.GetAuthorById(authorId);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var response = okResult.Value.Should().BeOfType<ApiResponse<AuthorResponse>>().Subject;
            
            response.Success.Should().BeTrue();
            response.Data.Should().NotBeNull();
            response.Data.Autor_id.Should().Be(authorId);
            response.Message.Should().Be("Se ha obtenido satisfactoriamente el autor");
            
            _mockAuthorService.Verify(s => s.GetAuthorByIdAsync(authorId), Times.Once);
        }

        #endregion

        #region CreateAuthor Tests

        [Fact]
        public async Task CreateAuthor_ReturnsCreatedAtAction_WithNewAuthor()
        {
            // Arrange
            var authorDto = new AuthorDto { Nombre = "Mario Vargas Llosa" };
            var createdAuthor = new CreateAuthorDto 
            { 
                AuthorId = 1, 
                Nombre = "Mario Vargas Llosa" 
            };

            _mockAuthorService
                .Setup(s => s.CreateAuthorAsync(authorDto))
                .ReturnsAsync(createdAuthor);

            // Act
            var result = await _controller.CreateAuthor(authorDto);

            // Assert
            var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdResult.ActionName.Should().Be(nameof(AuthorController.CreateAuthor));
            
            var response = createdResult.Value.Should().BeOfType<ApiResponse<CreateAuthorDto>>().Subject;
            response.Success.Should().BeTrue();
            response.Data.Should().NotBeNull();
            response.Data.AuthorId.Should().Be(1);
            
            _mockAuthorService.Verify(s => s.CreateAuthorAsync(authorDto), Times.Once);
        }

        #endregion

        #region UpdateAuthor Tests

        [Fact]
        public async Task UpdateAuthor_ReturnsOkResult_WithUpdatedAuthor()
        {
            // Arrange
            var authorId = 1;
            var updateDto = new UpdateAuthorDto { Nombre = "Gabriel García Márquez - Updated" };
            var updatedAuthor = new UpdateAuthorDto 
            { 
                Nombre = "Gabriel García Márquez - Updated" 
            };

            _mockAuthorService
                .Setup(s => s.UpdateAuthorAsync(authorId, updateDto))
                .ReturnsAsync(updatedAuthor);

            // Act
            var result = await _controller.UpdateAuthor(authorId, updateDto);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var response = okResult.Value.Should().BeOfType<ApiResponse<UpdateAuthorDto>>().Subject;
            
            response.Success.Should().BeTrue();
            response.Data.Should().NotBeNull();
            response.Message.Should().Be("Se ha actualizado correctamente el autor");
            
            _mockAuthorService.Verify(s => s.UpdateAuthorAsync(authorId, updateDto), Times.Once);
        }

        #endregion

        #region DeleteAuthor Tests

        [Fact]
        public async Task DeleteAuthor_ReturnsOkResult_WithTrue_WhenDeleteSuccessful()
        {
            // Arrange
            var authorId = 1;
            _mockAuthorService
                .Setup(s => s.DeleteAuthorAsync(authorId))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteAuthor(authorId);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var response = okResult.Value.Should().BeOfType<ApiResponse<bool>>().Subject;
            
            response.Success.Should().BeTrue();
            response.Data.Should().BeTrue();
            response.Message.Should().Be("Se ha eliminado correctamente");
            
            _mockAuthorService.Verify(s => s.DeleteAuthorAsync(authorId), Times.Once);
        }

        [Fact]
        public async Task DeleteAuthor_ReturnsOkResult_WithFalse_WhenDeleteFails()
        {
            // Arrange
            var authorId = 999;
            _mockAuthorService
                .Setup(s => s.DeleteAuthorAsync(authorId))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteAuthor(authorId);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var response = okResult.Value.Should().BeOfType<ApiResponse<bool>>().Subject;
            
            response.Data.Should().BeFalse();
        }

        #endregion
    }
}