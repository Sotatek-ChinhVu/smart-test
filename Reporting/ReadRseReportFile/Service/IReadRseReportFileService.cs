using Reporting.ReadRseReportFile.Model;

namespace Reporting.ReadRseReportFile.Service;

public interface IReadRseReportFileService
{
    JavaOutputData ReadFileRse(CoCalculateRequestModel inputModel);
}
