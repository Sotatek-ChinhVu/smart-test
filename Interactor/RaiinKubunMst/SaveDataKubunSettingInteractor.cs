using Domain.Models.RaiinKubunMst;
using Helper.Constants;
using Infrastructure.Repositories;
using UseCase.RaiinKubunMst.Save;

namespace Interactor.RaiinKubunMst
{
    public class SaveDataKubunSettingInteractor : ISaveDataKubunSettingInputPort
    {
        private readonly IRaiinKubunMstRepository _raiinKubunMstRepository;

        public SaveDataKubunSettingInteractor(IRaiinKubunMstRepository raiinKubunMstRepository)
        {
            _raiinKubunMstRepository = raiinKubunMstRepository;
        }

        public SaveDataKubunSettingOutputData Handle(SaveDataKubunSettingInputData inputData)
        {
            if (inputData.RaiinKubunMstModels != null && inputData.RaiinKubunMstModels.Any())
            {
                var result = _raiinKubunMstRepository.SaveDataKubunSetting(inputData.RaiinKubunMstModels);
                return new SaveDataKubunSettingOutputData(result);
            }
            return new SaveDataKubunSettingOutputData(new List<string>() { KubunSettingConstant.Nodata });
        }
    }
}
