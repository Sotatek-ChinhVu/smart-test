using UseCase.Core.Sync.Core;

namespace UseCase.Online.UpdateOnlineConsents;

public class UpdateOnlineConsentsInputData : IInputData<UpdateOnlineConsentsOutputData>
{
    public UpdateOnlineConsentsInputData(int userId, long ptId, List<string> responseList)
    {
        UserId = userId;
        PtId = ptId;
        ResponseList = responseList;
    }

    public int UserId { get; private set; }

    public long PtId { get; private set; }

    public List<string> ResponseList { get; private set; }
}
