
using Library.Common.Dto;

namespace Library.Application.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginDto dto);
    }
}