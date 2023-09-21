using UseCase.Core.Sync.Core;

namespace UseCase.MainMenu.SaveStaCsvMst;

public class SaveStaCsvMstInputData : IInputData<SaveStaCsvMstOutputData>
{
    public SaveStaCsvMstInputData(int hpId, int userId, int grpId, List<StatisticMenuItem> staticMenuList)
    {
        HpId = hpId;
        UserId = userId;
        StaticMenuList = staticMenuList;
        GrpId = grpId;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public List<StatisticMenuItem> StaticMenuList { get; private set; }
}
