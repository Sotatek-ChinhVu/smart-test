using EmrCloudApi.Responses.MstItem.Dto;

namespace EmrCloudApi.Responses.MstItem;

public class GetRenkeiTimingResponse
{
    public GetRenkeiTimingResponse(List<RenkeiTimingDto> renkeiTimingList)
    {
        RenkeiTimingList = renkeiTimingList;
    }

    public List<RenkeiTimingDto> RenkeiTimingList { get;private set; }
}
