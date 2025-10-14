using System.Net;
using System.Text.Json;
using Library.Api.Middleware;
using Library.Common.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Library.Tests.Middleware
{
    public class ExceptionMiddlewareTests
    {
        private readonly Mock<ILogger<ExceptionMiddleware>> _mockLogger;
        private readonly DefaultHttpContext _httpContext;

        public ExceptionMiddlewareTests()
        {
            _mockLogger = new Mock<ILogger<ExceptionMiddleware>>();
            _httpContext = new DefaultHttpContext();
            _httpContext.Response.Body = new MemoryStream();
        }

        #region Helper Methods

        private async Task<string> GetResponseBody()
        {
            _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(_httpContext.Response.Body);
            return await reader.ReadToEndAsync();
        }

        private ApiResponse<object> DeserializeResponse(string json)
        {
            return JsonSerializer.Deserialize<ApiResponse<object>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        }

        #endregion

        #region KeyNotFoundException Tests

        [Fact]
        public async Task InvokeAsync_ConKeyNotFoundException_RetornaNotFound()
        {
            // Arrange
            var expectedMessage = "Recurso no encontrado";
            RequestDelegate next = (HttpContext context) =>
                throw new KeyNotFoundException(expectedMessage);

            var middleware = new ExceptionMiddleware(next, _mockLogger.Object);

            // Act
            await middleware.InvokeAsync(_httpContext);

            // Assert
            Assert.Equal((int)HttpStatusCode.NotFound, _httpContext.Response.StatusCode);
            Assert.Equal("application/json", _httpContext.Response.ContentType);

            var responseBody = await GetResponseBody();
            var response = DeserializeResponse(responseBody);

            Assert.False(response.Success);
            Assert.Equal(expectedMessage, response.Message);
            Assert.Null(response.Data);

            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<KeyNotFoundException>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
                Times.Once);
        }

        #endregion

        #region ArgumentException Tests

        [Fact]
        public async Task InvokeAsync_ConArgumentException_RetornaBadRequest()
        {
            // Arrange
            var expectedMessage = "Argumento inválido";
            RequestDelegate next = (HttpContext context) =>
                throw new ArgumentException(expectedMessage);

            var middleware = new ExceptionMiddleware(next, _mockLogger.Object);

            // Act
            await middleware.InvokeAsync(_httpContext);

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, _httpContext.Response.StatusCode);
            Assert.Equal("application/json", _httpContext.Response.ContentType);

            var responseBody = await GetResponseBody();
            var response = DeserializeResponse(responseBody);

            Assert.False(response.Success);
            Assert.Equal(expectedMessage, response.Message);

            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<ArgumentException>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
                Times.Once);
        }

        #endregion

        #region InvalidOperationException Tests

        [Fact]
        public async Task InvokeAsync_ConInvalidOperationException_RetornaInternalServerError()
        {
            // Arrange
            var expectedMessage = "Operación inválida";
            RequestDelegate next = (HttpContext context) =>
                throw new InvalidOperationException(expectedMessage);

            var middleware = new ExceptionMiddleware(next, _mockLogger.Object);

            // Act
            await middleware.InvokeAsync(_httpContext);

            // Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, _httpContext.Response.StatusCode);
            Assert.Equal("application/json", _httpContext.Response.ContentType);

            var responseBody = await GetResponseBody();
            var response = DeserializeResponse(responseBody);

            Assert.False(response.Success);
            Assert.Equal(expectedMessage, response.Message);

            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
                Times.Once);
        }

        #endregion

        #region Success Cases

        [Fact]
        public async Task InvokeAsync_SinExcepciones_ContinuaNormalmente()
        {
            // Arrange
            var wasNextCalled = false;
            RequestDelegate next = (HttpContext context) =>
            {
                wasNextCalled = true;
                return Task.CompletedTask;
            };

            var middleware = new ExceptionMiddleware(next, _mockLogger.Object);

            // Act
            await middleware.InvokeAsync(_httpContext);

            // Assert
            Assert.True(wasNextCalled);
            _mockLogger.Verify(
                x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
                Times.Never);
        }

        #endregion

        #region DbUpdateException Tests

        [Fact]
        public async Task InvokeAsync_ConDbUpdateException_RetornaInternalServerError()
        {
            // Arrange
            RequestDelegate next = (HttpContext context) =>
                throw new DbUpdateException("Error de base de datos");

            var middleware = new ExceptionMiddleware(next, _mockLogger.Object);

            // Act
            await middleware.InvokeAsync(_httpContext);

            // Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, _httpContext.Response.StatusCode);
            Assert.Equal("application/json", _httpContext.Response.ContentType);

            var responseBody = await GetResponseBody();
            var response = DeserializeResponse(responseBody);

            Assert.False(response.Success);
            Assert.Equal("Error al guardar datos en la base de datos.", response.Message);

            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<DbUpdateException>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
                Times.Once);
        }

        #endregion

        #region Exception Tests

        [Fact]
        public async Task InvokeAsync_ConExcepcionGenerica_RetornaInternalServerError()
        {
            // Arrange
            RequestDelegate next = (HttpContext context) =>
                throw new Exception("Error inesperado");

            var middleware = new ExceptionMiddleware(next, _mockLogger.Object);

            // Act
            await middleware.InvokeAsync(_httpContext);

            // Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, _httpContext.Response.StatusCode);
            Assert.Equal("application/json", _httpContext.Response.ContentType);

            var responseBody = await GetResponseBody();
            var response = DeserializeResponse(responseBody);

            Assert.False(response.Success);
            Assert.Equal("Error interno del servidor.", response.Message);

            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
                Times.Once);
        }
        #endregion
    }
}