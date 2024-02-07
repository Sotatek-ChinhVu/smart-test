using EmrCloudApi.Responses.Yousiki.Dto;

namespace EmrCloudApi.Responses.Yousiki;

public class GetYousiki1InfDetailsByCodeNoResponse
{
    public GetYousiki1InfDetailsByCodeNoResponse(List<Yousiki1InfDetailDto> yousiki1InfDetailList)
    {
        Yousiki1InfDetailList = yousiki1InfDetailList;
    }

    public List<Yousiki1InfDetailDto> Yousiki1InfDetailList { get; private set; }
}
