using UseCase.Core.Sync.Core;

namespace UseCase.Online.UpdatePtInfOnlineQualify;

public class UpdatePtInfOnlineQualifyInputData : IInputData<UpdatePtInfOnlineQualifyOutputData>
{
    public UpdatePtInfOnlineQualifyInputData(int hpId, int userId, long ptId, List<PtInfConfirmationItem> resultList)
    {
        HpId = hpId;
        UserId = userId;
        PtId = ptId;
        ResultList = resultList;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public long PtId { get; private set; }

    public List<PtInfConfirmationItem> ResultList { get; private set; }
}
