using Domain.Models.SystemGenerationConf;
using UseCase.SystemGenerationConf;

namespace Interactor.SystemGenerationConf
{
    public class GetSystemGenerationConfInteractor : IGetSystemGenerationConfInputPort
    {
        private readonly ISystemGenerationConfRepository _systemGenerationConfRepository;

        public GetSystemGenerationConfInteractor(ISystemGenerationConfRepository systemGenerationConfRepository)
        {
            _systemGenerationConfRepository = systemGenerationConfRepository;
        }

        public GetSystemGenerationConfOutputData Handle(GetSystemGenerationConfInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0) return new GetSystemGenerationConfOutputData(0, GetSystemGenerationConfStatus.InvalidHpId);
                if (inputData.GrpCd <= 0) return new GetSystemGenerationConfOutputData(0, GetSystemGenerationConfStatus.InvalidGrpCd);
                if (inputData.GrpEdaNo < 0) return new GetSystemGenerationConfOutputData(0, GetSystemGenerationConfStatus.InvalidGrpEdaNo);
                if (inputData.PresentDate < 0) return new GetSystemGenerationConfOutputData(0, GetSystemGenerationConfStatus.InvalidPresentDate);
                if (inputData.DefaultValue < 0) return new GetSystemGenerationConfOutputData(0, GetSystemGenerationConfStatus.InvalidDefaultValue);

                var result = _systemGenerationConfRepository.GetSettingValue(inputData.HpId, inputData.GrpCd, inputData.GrpEdaNo, inputData.PresentDate, inputData.DefaultValue);
                return new GetSystemGenerationConfOutputData(result, GetSystemGenerationConfStatus.Successed);
            }
            catch
            {
                return new GetSystemGenerationConfOutputData(0, GetSystemGenerationConfStatus.Failed);
            }
        }
    }
}