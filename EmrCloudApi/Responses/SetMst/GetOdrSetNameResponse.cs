using EmrCloudApi.Responses.SetMst.Dto;

namespace EmrCloudApi.Responses.SetMst;

public class GetOdrSetNameResponse
{
    public GetOdrSetNameResponse(List<OdrSetNameDto> odrSetNameList)
    {
        OdrSetNameList = odrSetNameList;
    }

    public List<OdrSetNameDto> OdrSetNameList { get; private set; }
}
