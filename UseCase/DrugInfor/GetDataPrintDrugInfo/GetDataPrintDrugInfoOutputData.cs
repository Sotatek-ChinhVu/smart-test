using Domain.Models.DrugInfor;
using UseCase.Core.Sync.Core;

namespace UseCase.DrugInfor.GetDataPrintDrugInfo;

public class GetDataPrintDrugInfoOutputData : IOutputData
{
    public GetDataPrintDrugInfoOutputData(DrugInforModel drugInfor, string htmlData)
    {
        DrugInfor = drugInfor;
        HtmlData = htmlData;
    }

    public DrugInforModel DrugInfor { get; private set; }

    public string HtmlData { get; private set; }
}
