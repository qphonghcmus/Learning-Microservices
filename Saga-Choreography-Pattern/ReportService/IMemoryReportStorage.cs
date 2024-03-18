
namespace ReportService;

public interface IMemoryReportStorage
{
    void Add(Report report);
    IEnumerable<Report> Get();
}