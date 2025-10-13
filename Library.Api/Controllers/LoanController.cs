using Library.Common.Dto;
using Library.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class PrestamosController : ControllerBase
    {
        private readonly ILoanService _service;

        public PrestamosController(ILoanService service)
        {
            _service = service;
        }

        /// <summary>
        /// Actualiza la fecha de devolución de un préstamo.
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(ApiResponse<ActualizarDevolucionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ActualizarDevolucion(int id, [FromBody] ActualizarDevolucionDto dto)
        {
            var result = await _service.UpdateReturnDateAsync(id, dto.FechaDevolucion);

            var response = ApiResponse<ActualizarDevolucionDto>.SuccessResponse(
                result,
                "Préstamo actualizado correctamente"
            );

            return Ok(response);
        }

        /// <summary>
        /// Elimina un préstamo existente.
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EliminarPrestamo(int id)
        {
            var eliminado = await _service.DeleteLoanAsync(id);

            var response = ApiResponse<object>.SuccessResponse(
                new { eliminado },
                "Préstamo eliminado correctamente"
            );

            return Ok(response);
        }

        /// <summary>
        /// Obtiene todos los préstamos que no han sido devueltos.
        /// </summary>
        [HttpGet("no-devueltos")]
        [Authorize(Roles = "admin,user")]
        [ProducesResponseType(typeof(ApiResponse<List<PrestamoNoDevueltoDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ObtenerNoDevueltos()
        {
            var prestamos = await _service.ObtenerPrestamosNoDevueltosAsync();

            var response = ApiResponse<IEnumerable<PrestamoNoDevueltoDto>>.SuccessResponse(
                prestamos,
                "Listado de préstamos no devueltos"
            );

            return Ok(response);
        }
    }
}
