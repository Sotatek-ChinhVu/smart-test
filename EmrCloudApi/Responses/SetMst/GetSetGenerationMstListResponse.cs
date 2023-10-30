using EmrCloudApi.Responses.SetMst.Dto;

namespace EmrCloudApi.Responses.SetMst;

public class GetSetGenerationMstListResponse
{
    public GetSetGenerationMstListResponse(List<SetGenerationDto> setGenerationList)
    {
        SetGenerationList = setGenerationList;
    }

    public List<SetGenerationDto> SetGenerationList { get; private set; }
}
