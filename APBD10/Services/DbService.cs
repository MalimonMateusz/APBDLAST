using APBD10.Data;
using APBD10.Models;
using Microsoft.EntityFrameworkCore;

namespace APBD10.Services;

public class DbService : IDbService
{
    private readonly DatabaseContext _context;
    public DbService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<int> AddPrescriptionAsync(Prescription prescription)
    {
        
        Patient patient = await _context.Patients
            .FirstOrDefaultAsync(p => p.IdPatient == prescription.IdPatient);
            
        if (patient == null)
        {
            patient = new Patient
            {
                FirstName = prescription.Patient.FirstName,
                LastName = prescription.Patient.LastName,
                Birthdate = prescription.Patient.Birthdate
                
            };
            _context.Patients.Add(patient);
        }

      
        foreach (var prescriptionMedicament in prescription.PrescriptionMedicaments)
        {
            Medicament medicament = await _context.Medicaments
                .FirstOrDefaultAsync(m => m.IdMedicament == prescriptionMedicament.IdMedicament);

            if (medicament == null)
            {
                throw new Exception($"Medicament with ID {prescriptionMedicament.IdMedicament} does not exist.");
            }

            prescriptionMedicament.Medicament = medicament;
            patient.Prescriptions.Add(prescription);
        }

        if (prescription.DueDate < prescription.Date)
        {
            throw new Exception("DueDate must be greater than or equal to Date.");
        }

        await _context.SaveChangesAsync();
        return patient.IdPatient; 
    }

    public async Task<Patient> GetPatientByIdAsync(int id)
    {
        try
        {
            Patient patient = await _context.Patients
                .Include(p => p.Prescriptions)
                .ThenInclude(p => p.PrescriptionMedicaments) 
                .ThenInclude(pm => pm.Medicament)
                .Include(p => p.Prescriptions)
                .ThenInclude(p => p.Doctor)
                .FirstOrDefaultAsync(p => p.IdPatient == id);


            return patient;
        }
        catch (Exception ex)
        {
            
            throw new Exception($"Error while retrieving patient with ID {id}: {ex.Message}", ex);
        }
    }
}
