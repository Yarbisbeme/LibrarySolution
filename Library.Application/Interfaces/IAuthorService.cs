using Library.Application.DTOs;

namespace Library.Application.Interfaces
{
    public interface IAuthorService
    {
        Task<IEnumerable<AuthorDto>> GetAllAsync();
        Task<AuthorDto?> GetByIdAsync(int id);
    }
}
