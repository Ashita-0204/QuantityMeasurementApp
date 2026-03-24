using QuantityMeasurementModel.DTOs.Auth;

namespace QuantityMeasurementBusinessLayer.Services.Auth
{
    public interface IAuthService
    {
        AuthResponseDTO Register(RegisterRequestDTO request);
        AuthResponseDTO Login(LoginRequestDTO request);
    }
}