
using Library.Common.Dto;
using Library.Common.Dto.Autores;

namespace Library.Application.Interfaces
{
    public interface IAuthorService
    {
        // Obtener todos los autores
        public Task<List<AuthorResponse>> GetAllAuthorsAsync();
        // Obtener un autor por su ID
        public Task<AuthorResponse> GetAuthorByIdAsync(int authorId);
        // Crear un nuevo autor
        public Task<CreateAuthorDto> CreateAuthorAsync(AuthorDto authorDto);
        // Actualizar un autor existente
        public Task<UpdateAuthorDto> UpdateAuthorAsync(int authorId, UpdateAuthorDto authorDto);
        // Eliminar un autor por su ID
        public Task<bool> DeleteAuthorAsync(int authorId);
    }
}