using Domain.Models.MainMenu;
using UseCase.Core.Sync.Core;

namespace UseCase.MainMenu.SaveStaCsvMst;

public class SaveStaCsvMstInputData : IInputData<SaveStaCsvMstOutputData>
{
    public SaveStaCsvMstInputData(int hpId, int userId, List<StaCsvMstModel> staCsvModels)
    {
        HpId = hpId;
        UserId = userId;
        StaCsvModels = staCsvModels;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public List<StaCsvMstModel> StaCsvModels { get; private set; }
}
