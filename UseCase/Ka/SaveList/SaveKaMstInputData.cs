using UseCase.Core.Sync.Core;

namespace UseCase.Ka.SaveList;

public class SaveKaMstInputData : IInputData<SaveKaMstOutputData>
{
    public SaveKaMstInputData(int hpId, int userId, List<SaveKaMstInputItem> kaMstModels)
    {
        HpId = hpId;
        UserId = userId;
        KaMstModels = kaMstModels;
    }

    public int HpId { get; private set; }
    public int UserId { get; private set; }
    public List<SaveKaMstInputItem> KaMstModels { get; private set; }
}
