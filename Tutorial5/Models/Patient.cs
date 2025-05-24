using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Tutorial5.Models;

public class Patient
{
    [Key]
    public int IdPatient { get; set; }

    [Required, MaxLength(100)]
    public string FirstName { get; set; }

    [Required, MaxLength(100)]
    public string LastName { get; set; }

    [Required]
    [Column(TypeName = "date")]
    public DateTime BirthDate { get; set; }

    public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
}