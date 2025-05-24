namespace Tutorial5.Models.DTO;

public class PrescriptionDTO
{
    public int IdPrescription { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public DoctorDTO Doctor { get; set; }
    public List<PrescriptionMedicamentDTO> Medicaments { get; set; }
}