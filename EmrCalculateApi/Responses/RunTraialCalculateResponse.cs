using Domain.Models.Futan;
using CalculateService.Ika.Models;
using CalculateService.Receipt.Models;

namespace EmrCalculateApi.Responses
{
    public class RunTraialCalculateResponse
    {
        public RunTraialCalculateResponse(List<SinMeiDataModel> sinMeiList, List<KaikeiInfItemResponse> kaikeiInfList, List<CalcLogModel> calcLogList)
        {
            SinMeiList = sinMeiList;
            KaikeiInfList = kaikeiInfList;
            CalcLogList = calcLogList;
        }

        public List<SinMeiDataModel> SinMeiList { get; private set; }

        public List<KaikeiInfItemResponse> KaikeiInfList { get; private set; }

        public List<CalcLogModel> CalcLogList { get; private set; }
    }
}
