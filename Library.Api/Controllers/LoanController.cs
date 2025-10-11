using Library.Application.DTOs;
using Library.Application.Interfaces;
using Library.Common.Dtos;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ActualizarDevolucion(int id, [FromBody] ActualizarDevolucionDto dto)
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
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
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
        [ProducesResponseType(typeof(ApiResponse<List<PrestamoNoDevueltoDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
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
