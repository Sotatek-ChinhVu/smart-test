using Domain.Models.MstItem;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.SaveRenkei;

public class SaveRenkeiInputData : IInputData<SaveRenkeiOutputData>
{
    public SaveRenkeiInputData(int hpId, int userId, List<(int renkeiSbt, List<RenkeiConfModel> renkeiConfList)> renkeiTabList)
    {
        HpId = hpId;
        UserId = userId;
        RenkeiTabList = renkeiTabList;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public List<(int renkeiSbt, List<RenkeiConfModel> renkeiConfList)> RenkeiTabList { get; private set; }
}
