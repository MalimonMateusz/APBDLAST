using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD10.Models;

public class PrescriptionMedicament
{
    [Key, Column(Order = 0)]
    public int IdMedicament { get; set; }
    [Key, Column(Order = 1)]
    public int IdPrescription { get; set; }
    public int? Dose { get; set; }
    public string Details { get; set; }

  
    [ForeignKey("IdMedicament")]
    public Medicament Medicament { get; set; }
    [ForeignKey("IdPrescription")]
    public Prescription Prescription { get; set; }
}
