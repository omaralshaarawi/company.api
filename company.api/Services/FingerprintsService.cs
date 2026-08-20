using company.api.Data;
using company.api.Dto;
using company.api.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace company.api.Services
{
    public class FingerprintsService : IFingerprintsService
    {

        private readonly CompanyContext _context;

        public FingerprintsService(CompanyContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<FingerprintDto?> CreateFingerprintAsync(CreateFingerprintRequest request)
        {
            var employee = await _context.Employees.FindAsync(request.EmployeeId);
            if(employee == null) {
                return null;
            }
            var templateDataBytes = Encoding.UTF8.GetBytes(request.TemplateData);
            var qualityByte = request.Quality != null ? byte.Parse(request.Quality) : (byte?)null;
            var fingerprint = new Fingerprint
            {
                EmployeeId = request.EmployeeId,
                FingerIndex = request.FingerIndex,
                DeviceId = request.DeviceId,
                TemplateData = templateDataBytes,
                EnrolledDate = request.EnrolledDate,
                Quality = qualityByte
            };
            _context.Fingerprints.Add(fingerprint);
            await _context.SaveChangesAsync();
            return new FingerprintDto(
                fingerprint.FingerprintId,
                fingerprint.EmployeeId,
                fingerprint.FingerIndex,
                fingerprint.DeviceId,
                fingerprint.EnrolledDate,
                fingerprint.Quality
            );
        }

        public async Task<bool> DeleteFingerprintAsync(int fingerprintId)
        {
            var fingerprint = await _context.Fingerprints.FindAsync(fingerprintId);
            if (fingerprint == null) return false;
            _context.Fingerprints.Remove(fingerprint);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<FingerprintDto>?> GetFingerprintsAsync(int? employeeId)
        {
            if (employeeId == null)
            {
                return await _context.Fingerprints
                    .Select(f => new FingerprintDto(
                        f.FingerprintId,
                        f.EmployeeId,
                        f.FingerIndex,
                        f.DeviceId,
                        f.EnrolledDate,
                        f.Quality
                    ))
                    .ToListAsync();
            }
            else
            {
                var employee = await _context.Employees.FindAsync(employeeId);
                if (employee == null) return null;

                return await _context.Fingerprints
                    .Where(f => f.EmployeeId == employeeId)
                    .Select(f => new FingerprintDto(
                        f.FingerprintId,
                        f.EmployeeId,
                        f.FingerIndex,
                        f.DeviceId,
                        f.EnrolledDate,
                        f.Quality
                    ))
                    .ToListAsync();
            }
        }
        public async Task<FingerprintDto?> GetFingerprintAsyncById(int fingerprintId)
        {
            var fingerprint = await _context.Fingerprints.FindAsync(fingerprintId);
            if (fingerprint == null) return null;
            return new FingerprintDto(
                fingerprint.FingerprintId,
                fingerprint.EmployeeId,
                fingerprint.FingerIndex,
                fingerprint.DeviceId,
                fingerprint.EnrolledDate,
                fingerprint.Quality
            );
        }
    }
}
