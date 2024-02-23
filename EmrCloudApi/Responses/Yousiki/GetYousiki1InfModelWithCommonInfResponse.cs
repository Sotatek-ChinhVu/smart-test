using Domain.Models.Yousiki;
using EmrCloudApi.Responses.Yousiki.Dto;

namespace EmrCloudApi.Responses.Yousiki;

public class GetYousiki1InfModelWithCommonInfResponse
{
    public GetYousiki1InfModelWithCommonInfResponse(List<Yousiki1InfGetListDto> yousiki1InfList)
    {
        Yousiki1InfList = yousiki1InfList;
    }

    public List<Yousiki1InfGetListDto> Yousiki1InfList { get; private set; }
}
