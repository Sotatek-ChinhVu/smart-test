namespace Reporting.DrugInfo.Model;

public class DrugInfoData
{
    public DrugInfoData(int selectedFormType, int reportType, List<DrugInfoModel> drugInfoList)
    {
        this.selectedFormType = selectedFormType;
        this.reportType = reportType;
        this.drugInfoList = drugInfoList;
    }

    public DrugInfoData()
    {
        drugInfoList = new();
    }

    public int selectedFormType { get; private set; }

    public int reportType { get; private set; }

    public List<DrugInfoModel> drugInfoList { get; private set; }
}
