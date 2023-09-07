using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.GetPtInfModelsByRefNo;

public class GetPtInfModelsByRefNoInputData : IInputData<GetPtInfModelsByRefNoOutputData>
{
    public GetPtInfModelsByRefNoInputData(int hpId, long refNo)
    {
        HpId = hpId;
        RefNo = refNo;
    }

    public int HpId { get; private set; }

    public long RefNo { get; private set; }
}
