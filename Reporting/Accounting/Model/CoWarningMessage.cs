namespace Reporting.Accounting.Model;

public class CoWarningMessage
{
    public long PtNum { get; set; } = 0;
    public string WarningMessage { get; set; } = "";
    public List<string> Detail { get; set; } = new List<string>();
}
