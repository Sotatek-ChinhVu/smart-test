using UseCase.DrugInfor.GetDataPrintDrugInfo;

namespace EmrCloudApi.Requests.DrugInfor;

public class GetDataPrintDrugInfoRequest
{
    public int HpId { get; set; }

    public int SinDate { get; set; }

    public string ItemCd { get; set; } = string.Empty;

    public int Level { get; set; }

    public string YJCode { get; set; } = string.Empty;

    public TypeHTMLEnum Type { get; set; }
}
