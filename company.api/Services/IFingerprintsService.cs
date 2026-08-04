using company.api.Dto;

namespace company.api.Services
{
    public interface IFingerprintsService
    {
        Task<List<FingerprintDto>?> GetFingerprintAsyncByEmployeeId(int employeeId);
        Task<FingerprintDto?> GetFingerprintAsyncById(int fingerprintId);
        Task<FingerprintDto?> CreateFingerprintAsync(CreateFingerprintRequest request);
        Task<bool> DeleteFingerprintAsync(int fingerprintId);
    }
}
