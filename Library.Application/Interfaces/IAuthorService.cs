
using Library.Common.Dto;
using Library.Common.Dto.Autores;

namespace Library.Application.Interfaces
{
    public interface IAuthorService
    {
        // Obtener todos los autores
        public Task<ApiResponse<List<AuthorResponse>>> GetAllAuthorsAsync();
        // Obtener un autor por su ID
        public Task<ApiResponse<AuthorResponse>> GetAuthorByIdAsync(int authorId);
        // Crear un nuevo autor
        public Task<ApiResponse<CreateAuthorDto>> CreateAuthorAsync(AuthorDto authorDto);
        // Actualizar un autor existente
        public Task<ApiResponse<AuthorDto>> UpdateAuthorAsync(int authorId, AuthorDto authorDto);
        // Eliminar un autor por su ID
        public Task<ApiResponse<bool>> DeleteAuthorAsync(int authorId);
    }
}