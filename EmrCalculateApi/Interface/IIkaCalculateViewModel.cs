using EmrCalculateApi.Ika.Models;
using EmrCalculateApi.Receipt.Models;
using EmrCalculateApi.Requests;

namespace EmrCalculateApi.Interface
{
    public interface IIkaCalculateViewModel
    {
        void RunCalculateOne(int hpId, long ptId, int sinDate, int seikyuUp, string preFix);

        void RunCalculate(int hpId, long ptId, int sinDate, int seikyuUp, string preFix);

        void RunCalculateMonth(int hpId, int seikyuYm, List<long> ptIds, string preFix);
        (List<SinMeiDataModel>, List<Futan.Models.KaikeiInfModel>) RunTraialCalculate(List<OrderInfo> todayOdrInfs, ReceptionModel reception, bool calcFutan = true);
    }
}
