namespace Tutorial5.Models.DTO;

public class PatientDetailsDTO
{
    public int IdPatient { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public List<PrescriptionDTO> Prescriptions { get; set; }
}