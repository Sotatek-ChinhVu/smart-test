using UseCase.Core.Sync.Core;

namespace UseCase.Releasenote.UpdateListReleasenote;

public class UpdateListReleasenoteInputData : IInputData<UpdateListReleasenoteOutputData>
{
    public UpdateListReleasenoteInputData(int hpId, int userId, List<string> versions)
    {
        HpId = hpId;
        UserId = userId;
        Versions = versions;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public List<string> Versions { get; private set; }
}
