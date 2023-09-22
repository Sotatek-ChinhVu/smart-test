using Domain.Models.MstItem;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetRenkeiConf;

public class GetRenkeiConfOutputData : IOutputData
{
    public GetRenkeiConfOutputData(List<RenkeiConfModel> renkeiConfList, List<RenkeiMstModel> renkeiMstModelList, List<RenkeiTemplateMstModel> renkeiTemplateMstModelList, GetRenkeiConfStatus status)
    {
        RenkeiConfList = renkeiConfList;
        Status = status;
        RenkeiMstModelList = renkeiMstModelList;
        RenkeiTemplateMstModelList = renkeiTemplateMstModelList;
    }

    public List<RenkeiConfModel> RenkeiConfList { get; private set; }

    public List<RenkeiMstModel> RenkeiMstModelList { get; private set; }

    public List<RenkeiTemplateMstModel> RenkeiTemplateMstModelList { get; private set; }

    public GetRenkeiConfStatus Status { get; private set; }
}
