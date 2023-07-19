using Domain.Models.DrugInfor;
using System.Text.Json.Serialization;

namespace EmrCloudApi.Responses.DrugInfor;

public class GetDataPrintDrugInfoResponse
{
    public GetDataPrintDrugInfoResponse(DrugInforModel drugInfor, string htmlData)
    {
        DrugInfor = drugInfor;
        HtmlData = htmlData;
    }

    [JsonPropertyName("drugInfor")]
    public DrugInforModel DrugInfor { get; private set; }

    [JsonPropertyName("htmlData")]
    public string HtmlData { get; private set; }
}
