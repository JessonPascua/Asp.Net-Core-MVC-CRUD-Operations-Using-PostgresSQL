namespace Asp.Net_Core_MVC_CRUD_Operations_Using_PostgresSQL.Models
{
    public class PatientRecordViewModel
    {
        public int Id { get; set; }
        public string? FristName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? DoctorName { get; set; }
        public int DoctorId { get; set; }
        public string? Type { get; set; }
        public DateTime AppointmentDate { get; set; }
    }
}
