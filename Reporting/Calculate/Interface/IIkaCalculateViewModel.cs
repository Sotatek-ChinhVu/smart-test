using Reporting.Calculate.Ika.Models;
using Reporting.Calculate.Receipt.Models;
using Reporting.Calculate.Requests;

namespace Reporting.Calculate.Interface
{
    public interface IIkaCalculateViewModel
    {
        void RunCalculateOne(int hpId, long ptId, int sinDate, int seikyuUp, string preFix);

        void RunCalculate(int hpId, long ptId, int sinDate, int seikyuUp, string preFix);

        void RunCalculateMonth(int hpId, int seikyuYm, List<long> ptIds, string preFix);

        (List<SinMeiDataModel> sinMeis, List<Futan.Models.KaikeiInfModel> kaikeis, List<CalcLogModel> calcLogs) RunTraialCalculate(List<OrderInfo> todayOdrInfs, ReceptionModel reception, bool calcFutan = true);
    }
}
