namespace Tutorial5.Models.DTO;

public class PrescriptionInfoDTO
{
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public List<PrescriptionMedicamentDTO> Medicaments { get; set; }
}