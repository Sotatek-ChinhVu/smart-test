using CalculateService.Ika.Models;
using CalculateService.Receipt.Models;
using CalculateService.Requests;

namespace CalculateService.Interface
{
    public interface IIkaCalculateViewModel
    {
        void RunCalculateOne(int hpId, long ptId, int sinDate, int seikyuUp, string preFix);

        void RunCalculate(int hpId, long ptId, int sinDate, int seikyuUp, string preFix);

        void RunCalculateMonth(int hpId, int seikyuYm, List<long> ptIds, string preFix, string uniqueKey);

        (List<SinMeiDataModel> sinMeis, List<Futan.Models.KaikeiInfModel> kaikeis, List<CalcLogModel> calcLogs) RunTraialCalculate(List<OrderInfo> todayOdrInfs, ReceptionModel reception, bool calcFutan = true);

        void Dispose();
    }
}
