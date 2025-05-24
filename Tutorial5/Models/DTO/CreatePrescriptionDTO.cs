namespace Tutorial5.Models.DTO;

public class CreatePrescriptionDTO
{
    public DoctorDTO Doctor { get; set; }
    public PatientDTO Patient { get; set; }
    public PrescriptionInfoDTO Prescription { get; set; }
}