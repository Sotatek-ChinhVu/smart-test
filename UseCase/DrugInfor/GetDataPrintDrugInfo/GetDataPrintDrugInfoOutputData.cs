using Domain.Models.DrugInfor;
using System.Text.Json.Serialization;
using UseCase.Core.Sync.Core;

namespace UseCase.DrugInfor.GetDataPrintDrugInfo;

public class GetDataPrintDrugInfoOutputData : IOutputData
{
    public GetDataPrintDrugInfoOutputData(DrugInforModel drugInfor, string htmlData, string drugName)
    {
        DrugInfor = drugInfor;
        HtmlData = htmlData;
        DrugName = drugName;
    }

    [JsonPropertyName("drugInfor")]
    public DrugInforModel DrugInfor { get; private set; }

    [JsonPropertyName("htmlData")]
    public string HtmlData { get; private set; }

    [JsonPropertyName("drugName")]
    public string DrugName { get; private set; }
}
