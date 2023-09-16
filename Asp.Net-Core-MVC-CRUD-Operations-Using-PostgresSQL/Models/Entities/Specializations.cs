using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Asp.Net_Core_MVC_CRUD_Operations_Using_PostgresSQL.Models.Entities
{
    public class Specializations
    {
        public Specializations()
        {
            Physicians = new HashSet<Physicians>();
        }

        [Key]
        [PersonalData]
        [Column(TypeName = "INT")]
        public int SpecializationId { get; set; }

        [PersonalData]
        [Column(TypeName = "VARCHAR(100)")]
        public string? Type { get; set; }

        [PersonalData]
        [Column(TypeName = "boolean")] //From MySQL TINYINT to Postgress boolean.
        public bool Status { get; set; }

        public virtual ICollection<Physicians> Physicians { get; set; }
    }
}
