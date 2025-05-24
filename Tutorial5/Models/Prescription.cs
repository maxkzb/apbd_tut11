using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Tutorial5.Models;

public class Prescription
{
    [Key]
    public int IdPrescription { get; set; }

    [Required]
    [Column(TypeName = "date")]
    public DateTime Date { get; set; }

    [Required]
    [Column(TypeName = "date")]
    public DateTime DueDate { get; set; }

    [ForeignKey(nameof(Patient))]
    public int IdPatient { get; set; }
    public virtual Patient Patient { get; set; }

    [ForeignKey(nameof(Doctor))]
    public int IdDoctor { get; set; }
    public virtual Doctor Doctor { get; set; }

    public virtual ICollection<PrescriptionMedicament> PrescriptionMedicaments { get; set; } = new List<PrescriptionMedicament>();
}