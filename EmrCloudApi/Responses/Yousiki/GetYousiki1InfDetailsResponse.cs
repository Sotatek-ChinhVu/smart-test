using EmrCloudApi.Responses.Yousiki.Dto;

namespace EmrCloudApi.Responses.Yousiki;

public class GetYousiki1InfDetailsResponse
{
    public GetYousiki1InfDetailsResponse(Yousiki1InfDto yousiki1Inf)
    {
        Yousiki1Inf = yousiki1Inf;
    }

    public Yousiki1InfDto Yousiki1Inf { get; private set; }
}
