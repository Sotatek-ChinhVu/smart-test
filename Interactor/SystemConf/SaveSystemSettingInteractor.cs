using Domain.Models.HpInf;
using Domain.Models.SystemConf;
using Infrastructure.Repositories;
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
            if (inputData.IsUpdateHpInfo)
            {
                _hpInfRepository.SaveHpInf(inputData.UserId, inputData.HpInfs);
            }
        }
    }
}
