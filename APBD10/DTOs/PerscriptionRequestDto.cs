namespace APBD10.DTOs;

public class PrescriptionRequestDto
{
    public PatientDto Patient { get; set; }
    public List<MedicamentDto> Medicaments { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public DoctorDto Doctor { get; set; } // Dodanie właściwości dla lekarza
}

public class PatientDto
{
    public int IdPatient { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    // Możesz dodać inne właściwości pacjenta tutaj, jeśli są potrzebne
}

public class MedicamentDto
{
    public int IdMedicament { get; set; }
    public int Dose { get; set; }
    public string Description { get; set; }
    // Możesz dodać inne właściwości leku tutaj, jeśli są potrzebne
}

public class DoctorDto
{
    public int IdDoctor { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    // Możesz dodać inne właściwości lekarza tutaj, jeśli są potrzebne
}
