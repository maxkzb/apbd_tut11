using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Tutorial5.Models;

public class PrescriptionMedicament
{
    [Key, Column(Order = 0)]
    [ForeignKey(nameof(Medicament))]
    public int IdMedicament { get; set; }
    public virtual Medicament Medicament { get; set; }

    [Key, Column(Order = 1)]
    [ForeignKey(nameof(Prescription))]
    public int IdPrescription { get; set; }
    public virtual Prescription Prescription { get; set; }

    public int? Dose { get; set; }
    
    [Required, MaxLength(100)]
    public string Details { get; set; }
}