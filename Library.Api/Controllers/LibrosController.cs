using Library.Application.Interfaces;
using Library.Common.Dto;
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

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(ApiResponse<LibroResponseDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CrearLibro([FromBody] LibroCreateDto dto)
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

        [HttpGet("antes-de-2000")]
        [ProducesResponseType(typeof(ApiResponse<List<LibroResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<List<LibroResponseDto>>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObtenerLibrosAntesDe2000()
        {
            var libros = await _service.ObtenerLibrosAntesDe2000Async();
            return Ok(ApiResponse<IEnumerable<LibroResponseDto>>.SuccessResponse(libros));
        }
    }
}
