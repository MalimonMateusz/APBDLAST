using APBD10.Models;
using APBD10.Services;
using APBD10.Dtos;
using APBD10.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace APBD10.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IDbService _dbService;
        public PatientController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatient(int id)
        {
            try
            {
                // Pobranie pacjenta z serwisu
                Patient patient = await _dbService.GetPatientByIdAsync(id);

                if (patient == null)
                {
                    return NotFound($"Patient with ID {id} not found.");
                }

                // Mapowanie pacjenta na PatientDto
                var patientDto = new PatientDto
                {
                    IdPatient = patient.IdPatient,
                    FirstName = patient.FirstName,
                    LastName = patient.LastName,
                    Prescriptions = patient.Prescriptions
                        .OrderBy(p => p.DueDate)
                        .Select(p => new PrescriptionDto
                        {
                            IdPrescription = p.IdPrescription,
                            Date = p.Date.ToString("yyyy-MM-dd"),
                            DueDate = p.DueDate.ToString("yyyy-MM-dd"),
                            Medicaments = p.PrescriptionMedicaments.Select(pm => new MedicamentDto
                            {
                                IdMedicament = pm.IdMedicament,
                                Name = pm.Medicament.Name
                            }).ToList(),
                            Doctor = new DoctorDto
                            {
                                IdDoctor = p.Doctor.IdDoctor,
                                FirstName = p.Doctor.FirstName,
                                LastName = p.Doctor.LastName
                            }
                        }).ToList()
                };

                return Ok(patientDto);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to retrieve patient data: {ex.Message}");
            }
        }
    }
}
