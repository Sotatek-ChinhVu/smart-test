namespace Reporting.ReadRseReportFile.Model;

public class JavaOutputData
{
    public List<string> objectNames { get; set; } = new();
    public List<ObjectCalculateResponse> responses { get; set; } = new();
}
