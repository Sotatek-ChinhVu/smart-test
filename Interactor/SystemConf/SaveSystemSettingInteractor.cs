using Domain.Models.HpInf;
using Domain.Models.MstItem;
using Domain.Models.Santei;
using Domain.Models.SystemConf;
using UseCase.SystemConf.SaveSystemSetting;

namespace Interactor.SystemConf
{
    public class SaveSystemSettingInteractor : ISaveSystemSettingInputPort
    {
        private readonly ISystemConfRepository _systemConfRepository;
        private readonly IHpInfRepository _hpInfRepository;
        private readonly ISanteiInfRepository _santeiInfRepository;
        private readonly IMstItemRepository _mstItemRepository;

        public SaveSystemSettingInteractor(ISystemConfRepository systemConfRepository, IHpInfRepository hpInfRepository, ISanteiInfRepository santeiInfRepository, IMstItemRepository mstItemRepository)
        {
            _systemConfRepository = systemConfRepository;
            _hpInfRepository = hpInfRepository;
            _santeiInfRepository = santeiInfRepository;
            _mstItemRepository = mstItemRepository;
        }

        public SaveSystemSettingOutputData Handle(SaveSystemSettingInputData inputData)
        {
            try
            {
                if (inputData.HpInfs.Any())
                {
                    _hpInfRepository.SaveHpInf(inputData.UserId, inputData.HpInfs);
                }

                if (inputData.SystemConfMenus.Any())
                {
                    _systemConfRepository.SaveSystemGenerationConf(inputData.UserId, inputData.SystemConfMenus);
                    _systemConfRepository.SaveSystemSetting(inputData.HpId, inputData.UserId, inputData.SystemConfMenus);
                }

                if (inputData.SanteiInfs.Any())
                {
                    _santeiInfRepository.SaveAutoSanteiMst(inputData.HpId, inputData.UserId, inputData.SanteiInfs);
                }
                if (inputData.KensaCenters.Any())
                {
                    _mstItemRepository.SaveKensaCenterMst(inputData.UserId, inputData.KensaCenters);
                }

                return new SaveSystemSettingOutputData(SaveSystemSettingStatus.Successed);
            }
            finally
            {
                _hpInfRepository.ReleaseResource();
                _systemConfRepository.ReleaseResource();
                _santeiInfRepository.ReleaseResource();
                _mstItemRepository.ReleaseResource();
            }
        }
    }
}
