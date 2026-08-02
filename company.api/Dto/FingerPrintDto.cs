namespace company.api.Dto
{
    public class FingerprintDto
    {
        public int FingerprintId { get; set; }
        public int EmployeeId { get; set; }
        public string FingerIndex { get; set; } = null!;
        public string? DeviceId { get; set; }
        public DateTime? EnrolledDate { get; set; }
        public byte? Quality { get; set; }
    }
}
