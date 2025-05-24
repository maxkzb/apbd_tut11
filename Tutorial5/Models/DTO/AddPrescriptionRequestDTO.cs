namespace Tutorial5.Models.DTO;

public class AddPrescriptionRequestDTO
{
    public PatientDTO Patient { get; set; }
    public int IdDoctor { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public List<PrescriptionMedicamentCreateDTO> Medicaments { get; set; }
}