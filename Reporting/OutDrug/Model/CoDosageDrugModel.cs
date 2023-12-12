using Entity.Tenant;

namespace Reporting.OutDrug.Model;

public class CoDosageDrugModel
{
    public DosageDrug DosageDrug { get; private set; }

    public CoDosageDrugModel(DosageDrug dosageDrug)
    {
        DosageDrug = dosageDrug;
    }

    public string RikikaUnit
    {
        get
        {
            return DosageDrug.RikikaUnit ?? string.Empty;
        }
    }
    public decimal RikikaRate
    {
        get
        {
            return DosageDrug.RikikaRate;
        }
    }

    public string YakkaUnit
    {
        get
        {
            return DosageDrug.YakkaiUnit ?? string.Empty;
        }
    }

}
