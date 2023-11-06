using UseCase.Core.Sync.Core;

namespace UseCase.DrugInfor.GetDataPrintDrugInfo;

public class GetDataPrintDrugInfoInputData : IInputData<GetDataPrintDrugInfoOutputData>
{
    public GetDataPrintDrugInfoInputData(int hpId, int sinDate, string itemCd, int level, string drugName, string yJCode, TypeHTMLEnum type)
    {
        HpId = hpId;
        SinDate = sinDate;
        ItemCd = itemCd;
        Level = level;
        DrugName = drugName;
        YJCode = yJCode;
        Type = type;
    }

    public int HpId { get; private set; }

    public int SinDate { get; private set; }

    public string ItemCd { get; private set; }

    public int Level { get; private set; }

    public string DrugName { get; private set; }

    public string YJCode { get; private set; }

    public TypeHTMLEnum Type { get; private set; }
}
