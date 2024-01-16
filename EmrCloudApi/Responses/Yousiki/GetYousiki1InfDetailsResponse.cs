using EmrCloudApi.Responses.Yousiki.Dto;

namespace EmrCloudApi.Responses.Yousiki;

public class GetYousiki1InfDetailsResponse
{
    public GetYousiki1InfDetailsResponse(List<Yousiki1InfDetailDto> yousiki1InfDetailList)
    {
        Yousiki1InfDetailList = yousiki1InfDetailList;
    }

    public List<Yousiki1InfDetailDto> Yousiki1InfDetailList { get; private set; }
}
