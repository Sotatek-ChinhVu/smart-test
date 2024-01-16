using EmrCloudApi.Responses.Yousiki.Dto;

namespace EmrCloudApi.Responses.Yousiki
{
    public class GetHistoryYousikiResponse
    {
        public GetHistoryYousikiResponse(List<Yousiki1InfDto> yousiki1InfList)
        {
            Yousiki1InfList = yousiki1InfList;
        }

        public List<Yousiki1InfDto> Yousiki1InfList { get; private set; }
    }
}
