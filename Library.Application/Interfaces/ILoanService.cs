using Library.Application.DTOs;

namespace Library.Application.Interfaces
{
    public interface ILoanService
    {
        Task<IEnumerable<LoanDto>> GetUnreturnedLoansAsync();
        Task<bool> UpdateReturnDateAsync(int id, DateTime fecha_devolucion);
        Task<bool> DeleteLoanAsync(int id);
    }
}
