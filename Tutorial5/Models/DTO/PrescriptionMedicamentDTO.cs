namespace Tutorial5.Models.DTO;

public class PrescriptionMedicamentDTO
{
    public int IdMedicament { get; set; }
    public int IdPrescription { get; set; }
    public int? Dose { get; set; }
    public string Details { get; set; }
}