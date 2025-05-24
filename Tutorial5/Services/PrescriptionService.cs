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

    public async Task AddPrescriptionAsync(CreatePrescriptionDTO dto)
    {
        if (dto.Prescription.Medicaments.Count > 10)
            throw new ArgumentException("A prescription can contain a maximum of 10 medications.");

        if (dto.Prescription.DueDate < dto.Prescription.Date)
            throw new ArgumentException("DueDate must be greater than or equal to Date.");

        var medicamentIds = dto.Prescription.Medicaments.Select(m => m.IdMedicament).ToList();
        var existingMedicaments = await _context.Medicaments
            .Where(m => medicamentIds.Contains(m.IdMedicament))
            .Select(m => m.IdMedicament).ToListAsync();

        if (existingMedicaments.Count != medicamentIds.Count)
            throw new ArgumentException("One or more medicaments do not exist.");

        var doctor = await _context.Doctors.FirstOrDefaultAsync(d =>
            d.FirstName == dto.Doctor.FirstName &&
            d.LastName == dto.Doctor.LastName &&
            d.Email == dto.Doctor.Email);

        if (doctor == null)
        {
            doctor = new Doctor
            {
                FirstName = dto.Doctor.FirstName,
                LastName = dto.Doctor.LastName,
                Email = dto.Doctor.Email
            };
            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();
        }

        var patient = await _context.Patients.FirstOrDefaultAsync(p =>
            p.FirstName == dto.Patient.FirstName &&
            p.LastName == dto.Patient.LastName &&
            p.BirthDate == dto.Patient.Birthdate);

        if (patient == null)
        {
            patient = new Patient
            {
                FirstName = dto.Patient.FirstName,
                LastName = dto.Patient.LastName,
                BirthDate = dto.Patient.Birthdate,
            };
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
        }

        var prescription = new Prescription
        {
            Date = dto.Prescription.Date,
            DueDate = dto.Prescription.DueDate,
            IdDoctor = doctor.IdDoctor,
            IdPatient = patient.IdPatient,
            PrescriptionMedicaments = dto.Prescription.Medicaments.Select(m => new PrescriptionMedicament
            {
                IdMedicament = m.IdMedicament,
                Dose = m.Dose,
                Details = m.Details
            }).ToList()
        };

        _context.Prescriptions.Add(prescription);
        await _context.SaveChangesAsync();
    }

    public async Task<PatientResponseDTO> GetPatientInfoAsync(int patientId)
    {
        return await _context.Patients
            .Where(p => p.IdPatient == patientId)
            .Select(p => new PatientResponseDTO
            {
                IdPatient = p.IdPatient,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Birthdate = p.BirthDate,
                Prescriptions = p.Prescriptions
                    .OrderBy(pr => pr.DueDate)
                    .Select(pr => new PrescriptionDTO
                    {
                        IdPrescription = pr.IdPrescription,
                        Date = pr.Date,
                        DueDate = pr.DueDate,
                        Doctor = new DoctorDTO
                        {
                            FirstName = pr.Doctor.FirstName,
                            LastName = pr.Doctor.LastName,
                            Email = pr.Doctor.Email
                        },
                        Medicaments = pr.PrescriptionMedicaments.Select(pm => new PrescriptionMedicamentDto
                        {
                            IdMedicament = pm.IdMedicament,
                            Dose = pm.Dose,
                            Details = pm.Details
                        }).ToList()
                    }).ToList()
            }).FirstOrDefaultAsync();
    }
}