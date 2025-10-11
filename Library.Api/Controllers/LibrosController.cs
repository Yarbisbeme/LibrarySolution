using Library.Application.DTOs;
using Library.Application.Interfaces;
using Library.Common.Dtos;
using Library.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(ApiResponse<Book>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CrearLibro([FromBody] LibroCreateDto dto)
        {
            try
            {
                var libro = await _service.CrearLibroAsync(dto);
                return CreatedAtAction(nameof(CrearLibro), new { id = libro.LibroId }, libro);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "Error al guardar el libro en la base de datos",
                    new List<string> { "Ocurrió un error al procesar la solicitud. Intente nuevamente." }
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "Error interno del servidor: " + ex.Message,
                    new List<string> { "Ocurrió un error inesperado. Por favor contacte al administrador." }
                ));
            }
        }

        [HttpGet("antes-de-2000")]
        [ProducesResponseType(typeof(ApiResponse<List<LibroResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObtenerLibrosAntesDe2000()
        {
            var libros = await _service.ObtenerLibrosAntesDe2000Async();
            return Ok(libros);
        }
    }
}
