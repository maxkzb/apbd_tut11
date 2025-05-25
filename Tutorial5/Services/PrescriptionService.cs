using Microsoft.EntityFrameworkCore;
using Tutorial5.Data;
using Tutorial5.Models;
using Tutorial5.Models.DTO;

namespace Tutorial5.Services;

public class PrescriptionService : IPrescriptionService
{
    private readonly DatabaseContext _context;

    public PrescriptionService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<(bool Success, string Message, int? IdPrescription)> AddPrescriptionAsync(AddPrescriptionRequestDTO request)
    {
        if (request.Medicaments == null || request.Medicaments.Count == 0)
            return (false, "Prescription must include at least one medicament.", null);

        if (request.Medicaments.Count > 10)
            return (false, "Prescription cannot include more than 10 medicaments.", null);

        if (request.DueDate < request.Date)
            return (false, "DueDate must be >= Date.", null);

        var medicamentIds = request.Medicaments.Select(m => m.IdMedicament).ToList();
        var existingMedicaments = await _context.Medicaments
            .Where(m => medicamentIds.Contains(m.IdMedicament))
            .Select(m => m.IdMedicament)
            .ToListAsync();

        if (existingMedicaments.Count != medicamentIds.Count)
            return (false, "One or more medicaments do not exist.", null);

        var patient = await _context.Patients
            .FirstOrDefaultAsync(p =>
                p.FirstName == request.Patient.FirstName &&
                p.LastName == request.Patient.LastName &&
                p.BirthDate == request.Patient.BirthDate);

        if (patient == null)
        {
            patient = new Patient
            {
                FirstName = request.Patient.FirstName,
                LastName = request.Patient.LastName,
                BirthDate = request.Patient.BirthDate
            };
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
        }

        var doctorExists = await _context.Doctors.AnyAsync(d => d.IdDoctor == request.IdDoctor);
        if (!doctorExists)
            return (false, "Doctor does not exist.", null);

        var prescription = new Prescription
        {
            Date = request.Date,
            DueDate = request.DueDate,
            IdPatient = patient.IdPatient,
            IdDoctor = request.IdDoctor,
            PrescriptionMedicaments = request.Medicaments.Select(m => new PrescriptionMedicament
            {
                IdMedicament = m.IdMedicament,
                Dose = m.Dose,
                Details = m.Details
            }).ToList()
        };

        _context.Prescriptions.Add(prescription);
        await _context.SaveChangesAsync();

        return (true, null, prescription.IdPrescription);
    }
}

public interface IPatientService
{
    Task<PatientDetailsDTO> GetPatientDetailsAsync(int idPatient);
}

public class PatientService : IPatientService
{
    private readonly DatabaseContext _context;

    public PatientService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<PatientDetailsDTO> GetPatientDetailsAsync(int idPatient)
    {
        var patient = await _context.Patients
            .Include(p => p.Prescriptions)
                .ThenInclude(pr => pr.PrescriptionMedicaments)
                    .ThenInclude(pm => pm.Medicament)
            .Include(p => p.Prescriptions)
                .ThenInclude(pr => pr.Doctor)
            .FirstOrDefaultAsync(p => p.IdPatient == idPatient);

        if (patient == null)
            return null;

        return new PatientDetailsDTO
        {
            IdPatient = patient.IdPatient,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            BirthDate = patient.BirthDate,
            Prescriptions = patient.Prescriptions
                .OrderBy(pr => pr.DueDate)
                .Select(pr => new PrescriptionDTO
                {
                    IdPrescription = pr.IdPrescription,
                    Date = pr.Date,
                    DueDate = pr.DueDate,
                    Doctor = new DoctorDTO
                    {
                        IdDoctor = pr.Doctor.IdDoctor,
                        FirstName = pr.Doctor.FirstName,
                        LastName = pr.Doctor.LastName,
                        Email = pr.Doctor.Email
                    },
                    Medicaments = pr.PrescriptionMedicaments.Select(pm => new PrescriptionMedicamentDTO
                    {
                        IdMedicament = pm.IdMedicament,
                        Name = pm.Medicament.Name,
                        Description = pm.Medicament.Description,
                        Type = pm.Medicament.Type,
                        Dose = pm.Dose,
                        Details = pm.Details
                    }).ToList()
                })
                .ToList()
        };
    }
}