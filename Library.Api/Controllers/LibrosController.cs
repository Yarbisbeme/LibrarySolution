using Library.Application.Interfaces;
using Library.Common.Dto;
using Library.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LibrosController : ControllerBase
    {
        private readonly IBookService _service;

        public LibrosController(IBookService service)
        {
            _service = service;
        }

        #region Get antes-de-2000
        [HttpGet("antes-de-2000")]
        [ProducesResponseType(typeof(ApiResponse<List<LibroResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ObtenerLibrosAntesDe2000()
        {
            var libros = await _service.ObtenerLibrosAntesDe2000Async();
            return Ok(ApiResponse<IEnumerable<LibroResponseDto>>.SuccessResponse(libros));
        }
        #endregion

        #region GetBooksByAuthor
        [HttpGet("GetBooksByAuthor")]
        [ProducesResponseType(typeof(ApiResponse<List<LibroResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBooksByAuthor(string autor)
        {
            var response = await _service.ObtenerLibrosPorAutorAsync(autor);
            return Ok(ApiResponse<IEnumerable<LibroResponseDto>>.SuccessResponse(response, "Libro encontrado"));
        }
        #endregion

        #region GetListBooksByTitle
        [HttpGet("GetBooksByTitle")]
        [ProducesResponseType(typeof(ApiResponse<LibroResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetListBooksByTitle(string title)
        {
            var response = await _service.ObtenerLibroPorTituloAsync(title);
            return Ok(ApiResponse<IEnumerable<LibroResponseDto>>.SuccessResponse(response, "Libros encontrados"));
        }
        #endregion

        #region PostBook
        [HttpPost("PostBook")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(ApiResponse<LibroResponseDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CrearLibro([FromBody] LibroDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                throw new InvalidOperationException();
            }

            var libroCreado = await _service.CrearLibroAsync(dto);

            var response = ApiResponse<LibroResponseDto>.SuccessResponse(
                libroCreado,
                "Libro creado exitosamente"
            );

            return CreatedAtAction(nameof(ObtenerLibrosAntesDe2000), new { id = libroCreado.LibroId }, response);
        }
        #endregion

        #region DeleteBook
        [HttpDelete("DeleteBook")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(ApiResponse<LibroResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var response = await _service.EliminarLibroAsync(id);
            return Ok(ApiResponse<bool>.SuccessResponse(response, "Se ha eliminado el libro"));
        }
        #endregion

        #region UpdateBook
        [HttpPut("UpdateBook")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(ApiResponse<LibroResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateBook(int id, LibroDto dto)
        {
            var libro = new LibroDto
            {
                Titulo = dto.Titulo,
                AnioPublicacion = dto.AnioPublicacion,
                Genero = dto.Genero,
            };
            var response = await _service.ActualizarLibroAsync(id, libro);
            return Ok(ApiResponse<LibroResponseDto>.SuccessResponse(response, "Se ha actualizado correctamente el libro"));
        } 
        #endregion

    }
}
