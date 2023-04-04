using Domain.Models.SystemConf;
using UseCase.SystemConf.SystemSetting;

namespace Interactor.SystemConf
{
    public class GetSystemSettingInteractor : IGetSystemSettingInputPort
    {
        private readonly ISystemConfRepository _systemConfRepository;

        public GetSystemSettingInteractor(ISystemConfRepository systemConfRepository)
        {
            _systemConfRepository = systemConfRepository;
        }

        public GetSystemSettingOutputData Handle(GetSystemSettingInputData inputData)
        {
            try
            {
                var systemSetting = _systemConfRepository.GetListSystemConfMenuWithGeneration(inputData.HpId, 2000);

                return new GetSystemSettingOutputData(systemSetting, GetSystemSettingStatus.Successed);
            }
            finally
            {
                _systemConfRepository.ReleaseResource();
            }
        }
    }
}
