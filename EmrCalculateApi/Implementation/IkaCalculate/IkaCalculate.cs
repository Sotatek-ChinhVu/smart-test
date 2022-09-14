using EmrCalculateApi.Futan.Models;
using EmrCalculateApi.Interface;

namespace EmrCalculateApi.Implementation.IkaCalculate
{
    public class IkaCalculate : IIkaCalculate
    {
        public void RunCalculate(int hpId, long ptId, int sinDate, int seikyuUp, string preFix)
        {
            throw new NotImplementedException();
        }

        public void RunCalculateMonth(int hpId, int seikyuYm, List<long> ptIds, string preFix)
        {
            throw new NotImplementedException();
        }

        public void RunCalculateOne(int hpId, long ptId, int sinDate, int seikyuUp, string preFix)
        {
            throw new NotImplementedException();
        }
    }
}
