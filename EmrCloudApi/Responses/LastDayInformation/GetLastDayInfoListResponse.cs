using EmrCloudApi.Responses.LastDayInformation.Dto;

namespace EmrCloudApi.Responses.LastDayInformation;

public class GetLastDayInfoListResponse
{
    public GetLastDayInfoListResponse(List<OdrDateInfDto> odrDateInfList)
    {
        OdrDateInfList = odrDateInfList;
    }

    public List<OdrDateInfDto> OdrDateInfList { get; private set; }
}
