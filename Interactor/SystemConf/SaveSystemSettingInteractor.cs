using Domain.Models.HpInf;
using Domain.Models.SystemConf;
using UseCase.SystemConf.SaveSystemSetting;

namespace Interactor.SystemConf
{
    public class SaveSystemSettingInteractor : ISaveSystemSettingInputPort
    {
        private readonly ISystemConfRepository _systemConfRepository;
        private readonly IHpInfRepository _hpInfRepository;

        public SaveSystemSettingInteractor(ISystemConfRepository systemConfRepository, IHpInfRepository hpInfRepository)
        {
            _systemConfRepository = systemConfRepository;
            _hpInfRepository = hpInfRepository;
        }

        public SaveSystemSettingOutputData Handle(SaveSystemSettingInputData inputData)
        {
            try
            {
                if (inputData.IsUpdateHpInfo)
                {
                    _hpInfRepository.SaveHpInf(inputData.UserId, inputData.HpInfs);
                }

                if (inputData.IsUpdateSystemGenerationConf)
                {
                    _systemConfRepository.SaveSystemGenerationConf(inputData.UserId, inputData.SystemConfMenus);
                }


                return new SaveSystemSettingOutputData(SaveSystemSettingStatus.Successed);
            }
            finally
            {
                _hpInfRepository.ReleaseResource();
                _systemConfRepository.ReleaseResource();
            }
        }
    }
}
