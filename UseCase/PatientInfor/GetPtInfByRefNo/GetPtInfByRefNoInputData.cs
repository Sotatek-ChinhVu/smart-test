using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.GetPtInfByRefNo;

public class GetPtInfByRefNoInputData : IInputData<GetPtInfByRefNoOutputData>
{
    public GetPtInfByRefNoInputData(int hpId, long refNo)
    {
        HpId = hpId;
        RefNo = refNo;
    }

    public int HpId { get; private set; }

    public long RefNo { get; private set; }
}
