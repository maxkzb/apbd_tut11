using Tutorial5.Models.DTO;

namespace Tutorial5.Services;

public interface IPrescriptionService
{
    Task<(bool Success, string Message, int? IdPrescription)> AddPrescriptionAsync(AddPrescriptionRequestDTO request);
}