using APBD10.Data;
using APBD10.DTOs;
using APBD10.Models;
using APBD10.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace APBD10.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionController : ControllerBase
    {
        private readonly IDbService _dbService;
        private readonly DatabaseContext _context;

        public PrescriptionController(IDbService dbService, DatabaseContext context)
        {
            _dbService = dbService;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePrescription(PrescriptionRequestDto requestDto)
        {
            try
            {
               
                Patient patient = await _context.Patients
                    .FirstOrDefaultAsync(p => p.IdPatient == requestDto.Patient.IdPatient);

                if (patient == null)
                {
                    patient = new Patient
                    {
                        FirstName = requestDto.Patient.FirstName,
                        LastName = requestDto.Patient.LastName,
                       
                    };
                    _context.Patients.Add(patient);
                }

                
                foreach (var med in requestDto.Medicaments)
                {
                    Medicament medicament = await _context.Medicaments
                        .FirstOrDefaultAsync(m => m.IdMedicament == med.IdMedicament);

                    if (medicament == null)
                    {
                        return BadRequest($"Medicament with ID {med.IdMedicament} does not exist.");
                    }
                }

              
                if (requestDto.Medicaments.Count > 10)
                {
                    return BadRequest("Prescription can include maximum 10 medicaments.");
                }

              
                if (requestDto.DueDate < requestDto.Date)
                {
                    return BadRequest("DueDate must be greater than or equal to Date.");
                }

         
                Prescription prescription = new Prescription
                {
                    Date = requestDto.Date,
                    DueDate = requestDto.DueDate,
                    IdPatient = patient.IdPatient,
                    IdDoctor = requestDto.Doctor.IdDoctor,
                    PrescriptionMedicaments = requestDto.Medicaments.Select(m => new PrescriptionMedicament
                    {
                        IdMedicament = m.IdMedicament,
                        Dose = m.Dose,
                        Details = m.Description
                    }).ToList()
                };

            
                int patientId = await _dbService.AddPrescriptionAsync(prescription);

                return Ok($"Prescription added successfully for patient with ID: {patientId}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to add prescription: {ex.Message}");
            }
        }
    }
}
