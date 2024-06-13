using APBD10.Models;
using System;
using System.Threading.Tasks;

namespace APBD10.Services
{
    public interface IDbService
    {
        Task<int> AddPrescriptionAsync(Prescription prescription); 
        Task<Patient> GetPatientByIdAsync(int id);
    }
}