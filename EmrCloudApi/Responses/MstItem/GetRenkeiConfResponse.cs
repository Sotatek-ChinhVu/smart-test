using EmrCloudApi.Responses.MstItem.Dto;

namespace EmrCloudApi.Responses.MstItem;

public class GetRenkeiConfResponse
{
    public GetRenkeiConfResponse(List<RenkeiConfDto> renkeiConfModelList)
    {
        RenkeiConfModelList = renkeiConfModelList;
    }

    public List<RenkeiConfDto> RenkeiConfModelList { get; private set; }
}
