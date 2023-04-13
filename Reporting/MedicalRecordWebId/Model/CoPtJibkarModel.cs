using Entity.Tenant;

namespace Reporting.MedicalRecordWebId.Model;

public class CoPtJibkarModel
{
    public PtJibkar PtJibkar { get; }

    public CoPtJibkarModel(PtJibkar ptJibkar)
    {
        PtJibkar = ptJibkar;
    }

    public string WebId => PtJibkar.WebId ?? string.Empty;
}
