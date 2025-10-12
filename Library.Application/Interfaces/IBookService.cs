
using Library.Common.Dto;

namespace Library.Application.Interfaces
{
    public interface IBookService
    {
        Task<LibroResponseDto> CrearLibroAsync(LibroCreateDto dto);
        Task<IEnumerable<LibroResponseDto>> ObtenerLibrosAntesDe2000Async();
    }
}
