using Domain.Models.RaiinKubunMst;
using UseCase.RaiinKubunMst.LoadData;

namespace Interactor.RaiinKubunMst
{
    public class LoadDataKubunSettingInteractor : ILoadDataKubunSettingInputPort
    {
        private readonly IRaiinKubunMstRepository _raiinKubunMstRepository;
        public LoadDataKubunSettingInteractor(IRaiinKubunMstRepository raiinKubunMstRepository)
        {
            _raiinKubunMstRepository = raiinKubunMstRepository;
        }

        public LoadDataKubunSettingOutputData Handle(LoadDataKubunSettingInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0) return new LoadDataKubunSettingOutputData(LoadDataKubunSettingStatus.InvalidHpId);

                List<RaiinKubunMstModel> raiinKubunList = _raiinKubunMstRepository.LoadDataKubunSetting(inputData.HpId, inputData.UserId);
                return new LoadDataKubunSettingOutputData(raiinKubunList, LoadDataKubunSettingStatus.Successed);
            }
            finally
            {
                _raiinKubunMstRepository.ReleaseResource();
            }
        }
    }
}
