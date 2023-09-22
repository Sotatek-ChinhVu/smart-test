using EmrCloudApi.Responses.MstItem.Dto;

namespace EmrCloudApi.Responses.MstItem;

public class GetRenkeiConfResponse
{
    public GetRenkeiConfResponse(List<RenkeiConfDto> renkeiConfModelList, List<RenkeiMstDto> renkeiMstModelList, List<RenkeiTemplateMstDto> renkeiTemplateMstModellList)
    {
        RenkeiConfModelList = renkeiConfModelList;
        RenkeiMstModelList = renkeiMstModelList;
        RenkeiTemplateMstModellList = renkeiTemplateMstModellList;
    }

    public List<RenkeiConfDto> RenkeiConfModelList { get; private set; }

    public List<RenkeiMstDto> RenkeiMstModelList { get; private set; }

    public List<RenkeiTemplateMstDto> RenkeiTemplateMstModellList { get; private set; }
}
