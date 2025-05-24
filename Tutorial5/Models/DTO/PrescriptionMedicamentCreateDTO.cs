namespace Tutorial5.Models.DTO;

public class PrescriptionMedicamentCreateDTO
{
    public int IdMedicament { get; set; }
    public int? Dose { get; set; }
    public string Details { get; set; }
}