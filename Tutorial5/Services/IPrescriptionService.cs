using Tutorial5.Models.DTO;

namespace Tutorial5.Services;

public interface IPrescriptionService
{
    Task AddPrescriptionAsync(CreatePrescriptionDTO dto);
    Task<PatientResponseDTO> GetPatientInfoAsync(int patientId);
}