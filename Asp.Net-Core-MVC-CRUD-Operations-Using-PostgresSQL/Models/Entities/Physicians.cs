using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Asp.Net_Core_MVC_CRUD_Operations_Using_PostgresSQL.Models.Entities
{
    public class Physicians
    {
        public Physicians()
        {
            Patients = new HashSet<Patients>();
        }

        [Key]
        [PersonalData]
        [Column(TypeName = "INT")]
        public int DoctorId { get; set; }

        [PersonalData]
        [Column(TypeName = "VARCHAR(100)")]
        public string? DoctorFirstName { get; set; }

        [PersonalData]
        [Column(TypeName = "VARCHAR(100)")]
        public string? DoctorLastName { get; set; }

        [PersonalData]
        [Column(TypeName = "INT")]
        public int SpecializationId { get; set; }


        [NotMapped] // This attribute ensures that this property is not mapped to the database
        public string DoctorFullName => $"{DoctorFirstName} {DoctorLastName}";

        [ForeignKey("SpecializationId")]  // Referenced table.
        public virtual Specializations Specialization { get; set; } = null!;
        public virtual ICollection<Patients> Patients { get; set; }
    }
}
