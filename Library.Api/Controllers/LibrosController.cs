using Library.Application.Interfaces;
using Library.Common.Dtos;
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
        public async Task<IActionResult> CrearLibro([FromBody] LibroCreateDto dto)
        {
            try
            {
                var libro = await _service.CrearLibroAsync(dto);
                return CreatedAtAction(nameof(CrearLibro), new { id = libro.LibroId }, libro);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch
            {
                return StatusCode(500, new { message = "Error al crear el libro" });
            }
        }

        [HttpGet("antes-de-2000")]
        public async Task<IActionResult> ObtenerLibrosAntesDe2000()
        {
            var libros = await _service.ObtenerLibrosAntesDe2000Async();
            return Ok(libros);
        }
    }
}
