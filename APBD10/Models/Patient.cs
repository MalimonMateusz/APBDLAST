using System.ComponentModel.DataAnnotations;

namespace APBD10.Models;

public class Patient
{
    [Key]
    public int IdPatient { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime Birthdate { get; set; }

    // Navigation property
    public ICollection<Prescription> Prescriptions { get; set; }
}
