using Library.Application.DTOs;

namespace Library.Application.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<BookDto>> GetBooksBeforeYearAsync(int year);
        Task<BookDto> CreateBookAsync(CreateBookRequest request);
    }
}
