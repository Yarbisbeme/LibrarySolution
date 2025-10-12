using Library.Common.Dto;

namespace Library.Application.Interfaces
{
    public interface ILoanService
    {
        Task<IEnumerable<PrestamoNoDevueltoDto>> ObtenerPrestamosNoDevueltosAsync();
        Task<ActualizarDevolucionDto> UpdateReturnDateAsync(int id, DateTime fecha_devolucion);
        Task<bool> DeleteLoanAsync(int id);
        Task<bool> PostLoan(int id);
    }
}
