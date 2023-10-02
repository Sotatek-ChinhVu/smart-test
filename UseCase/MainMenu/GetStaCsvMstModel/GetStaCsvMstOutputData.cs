using Domain.Models.MainMenu;
using UseCase.Core.Sync.Core;

namespace UseCase.MainMenu.GetStaCsvMstModel;

public class GetStaCsvMstOutputData : IOutputData
{
    public GetStaCsvMstOutputData(List<StaCsvMstModel> staCsvMstModels, GetStaCsvMstStatus status)
    {
        StaCsvMstModels = staCsvMstModels;
        Status = status;
    }

    public List<StaCsvMstModel> StaCsvMstModels { get; private set; }

    public GetStaCsvMstStatus Status { get; private set; }
}
