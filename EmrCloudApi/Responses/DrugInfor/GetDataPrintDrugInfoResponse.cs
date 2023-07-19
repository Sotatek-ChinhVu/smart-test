using Domain.Models.DrugInfor;
using System.Text.Json.Serialization;

namespace EmrCloudApi.Responses.DrugInfor;

public class GetDataPrintDrugInfoResponse
{
    public GetDataPrintDrugInfoResponse(DrugInforModel drugInfor, string htmlData, int drugType)
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
