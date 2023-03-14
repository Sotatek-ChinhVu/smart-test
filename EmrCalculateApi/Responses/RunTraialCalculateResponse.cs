using Domain.Models.Futan;
using EmrCalculateApi.Receipt.Models;

namespace EmrCalculateApi.Responses
{
    public class RunTraialCalculateResponse
    {
        public RunTraialCalculateResponse(List<SinMeiDataModel> sinMeiList, List<KaikeiInfItemResponse> kaikeiInfList)
        {
            SinMeiList = sinMeiList;
            KaikeiInfList = kaikeiInfList;
        }

        public List<SinMeiDataModel> SinMeiList { get; private set; }

        public List<KaikeiInfItemResponse> KaikeiInfList { get; private set; }
    }
}
