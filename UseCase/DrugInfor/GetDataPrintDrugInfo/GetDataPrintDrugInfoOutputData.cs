using Domain.Models.DrugInfor;
using System.Text.Json.Serialization;
using UseCase.Core.Sync.Core;

namespace UseCase.DrugInfor.GetDataPrintDrugInfo;

public class GetDataPrintDrugInfoOutputData : IOutputData
{
    public GetDataPrintDrugInfoOutputData(DrugInforModel drugInfor, string htmlData, int drugType)
    {
        DrugInfor = drugInfor;
        HtmlData = htmlData;
        DrugType = drugType;
    }

    [JsonPropertyName("drugInfor")]
    public DrugInforModel DrugInfor { get; private set; }

    [JsonPropertyName("htmlData")]
    public string HtmlData { get; private set; }

    [JsonPropertyName("drugType")]
    public int DrugType { get; private set; }
}
