using Library.Application.DTOs;
using Library.Application.Interfaces;
using Library.Common.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrestamosController : ControllerBase
    {
        private readonly ILoanService _service;

        public PrestamosController(ILoanService service)
        {
            _service = service;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarPrestamo(int id, [FromBody] ActualizarDevolucionDto dto)
        {
            try
            {
                var result = await _service.UpdateReturnDateAsync(id, dto.FechaDevolucion);
                return Ok(new
                {
                    message = "Préstamo actualizado correctamente",
                    result
                });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Préstamo no encontrado" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al actualizar el préstamo: {ex.Message}" });
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarPrestamo(int id)
        {
            try
            {
                var eliminado = await _service.DeleteLoanAsync(id);
                return StatusCode(200, new { message = "Prestamo Eliminado" });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Préstamo no encontrado" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al eliminar el préstamo: {ex.Message}" });
            }
        }

        [HttpGet("no-devueltos")]
        public async Task<IActionResult> ObtenerNoDevueltos()
        {
            try
            {
                var prestamos = await _service.ObtenerPrestamosNoDevueltosAsync();
                return Ok(prestamos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al obtener los préstamos no devueltos: {ex.Message}" });
            }
        }
    }
}
