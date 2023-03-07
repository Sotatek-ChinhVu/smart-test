using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.SaveReceCheckOpt;

public class SaveReceCheckOptInputData : IInputData<SaveReceCheckOptOutputData>
{
    public SaveReceCheckOptInputData(int hpId, int userId, List<ReceCheckOptItem> receCheckOptList)
    {
        HpId = hpId;
        UserId = userId;
        ReceCheckOptList = receCheckOptList;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public List<ReceCheckOptItem> ReceCheckOptList { get; private set; }
}
