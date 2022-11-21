using EmrCalculateApi.Interface;

namespace EmrCalculateApi.Ika.ViewModels
{
    public class IkaCalculateViewModel : IIkaCalculateViewModel
    {
        private readonly IFutancalcViewModel _iFutancalcViewModel;
        public IkaCalculateViewModel(IFutancalcViewModel iFutancalcViewModel)
        {
            _iFutancalcViewModel = iFutancalcViewModel;
        }

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
