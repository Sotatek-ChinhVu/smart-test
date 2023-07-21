using UseCase.Core.Sync.Core;

namespace UseCase.DrugInfor.Get;

public class GetDrugInforInputData : IInputData<GetDrugInforOutputData>
{
    public GetDrugInforInputData(int hpId, int sinDate, string itemCd)
    {
        HpId = hpId;
        SinDate = sinDate;
        ItemCd = itemCd;
    }

    public int HpId { get; private set; }

    public int SinDate { get; private set; }

    public string ItemCd { get; private set; }
}
