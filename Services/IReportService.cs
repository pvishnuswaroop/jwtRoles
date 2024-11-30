using QuickServeAPP.DTOs;

namespace QuickServeAPP.Services
{
    public interface IReportService
    {
        Task<SystemReportDto> GenerateSystemReportAsync();
    }
}
