using UseCase.Core.Sync.Core;

namespace UseCase.ReleasenoteRead;

public class GetListReleasenoteReadInputData : IInputData<GetListReleasenoteReadOutputData>
{
    public GetListReleasenoteReadInputData(int hpId, int userId)
    {
        HpId = hpId;
        UserId = userId;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }
}
