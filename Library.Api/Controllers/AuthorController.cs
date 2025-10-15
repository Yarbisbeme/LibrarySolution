
using Library.Application.Interfaces;
using Library.Common.Dto;
using Library.Common.Dto.Autores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;
        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        /// <summary>
        /// Obtiene todos los autores.
        /// </summary>
        [HttpGet("GetAuthors")]
        [ProducesResponseType(typeof(ApiResponse<List<AuthorResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAuthors()
        {
            var authors = await _authorService.GetAllAuthorsAsync();
            return Ok(ApiResponse<IEnumerable<AuthorResponse>>.SuccessResponse(authors, "Se han obtenido satisfactoriamente todos los autores"));
        }

        /// <summary>
        /// Obtiene un autor por su ID.
        /// </summary>
        [HttpGet("GetAuthorById/{authorId}")]
        [ProducesResponseType(typeof(ApiResponse<AuthorResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAuthorById(int authorId)
        {
            var author = await _authorService.GetAuthorByIdAsync(authorId);
            return Ok(ApiResponse<AuthorResponse>.SuccessResponse(author, "Se ha obtenido satisfactoriamente el autor"));
        }

        /// <summary>
        /// Crea un nuevo autor.
        /// </summary>
        [HttpPost("CreateAuthor")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(ApiResponse<AuthorDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateAuthor([FromBody] AuthorDto authorDto)
        {
            var createdAuthor = await _authorService.CreateAuthorAsync(authorDto);
            return CreatedAtAction(nameof(CreateAuthor), new { authorId = createdAuthor.AuthorId }, ApiResponse<CreateAuthorDto>.SuccessResponse(createdAuthor, "Se ha creado correctamente e autor"));
        }

        /// <summary>
        /// Actualiza un autor existente.
        /// </summary>
        [HttpPut("UpdateAuthor/{authorId}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(ApiResponse<AuthorDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAuthor(int authorId, [FromBody] UpdateAuthorDto authorDto)
        {
            var updatedAuthor = await _authorService.UpdateAuthorAsync(authorId, authorDto);
            return Ok(ApiResponse<UpdateAuthorDto>.SuccessResponse(updatedAuthor, "Se ha actualizado correctamente el autor"));
        }

        /// <summary>
        /// Elimina un autor por su ID.
        /// </summary>
        [HttpDelete("DeleteAuthor/{authorId}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteAuthor(int authorId)
        {
            var result = await _authorService.DeleteAuthorAsync(authorId);
            return Ok(ApiResponse<bool>.SuccessResponse(result, "Se ha eliminado correctamente"));
        }
    }
}