using UseCase.Core.Sync.Core;

namespace UseCase.Online.UpdateOnlineConsents;

public class UpdateOnlineConsentsInputData : IInputData<UpdateOnlineConsentsOutputData>
{
    public UpdateOnlineConsentsInputData(int hpId, int userId, long ptId, List<string> responseList)
    {
        HpId = hpId;
        UserId = userId;
        PtId = ptId;
        ResponseList = responseList;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public long PtId { get; private set; }

    public List<string> ResponseList { get; private set; }
}
