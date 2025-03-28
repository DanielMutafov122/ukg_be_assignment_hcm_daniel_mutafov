using Application.DTOs;

namespace Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto?> Authenticate(LoginRequestDto request);
}
