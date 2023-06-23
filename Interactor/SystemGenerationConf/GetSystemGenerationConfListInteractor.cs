using Domain.Models.SystemGenerationConf;
using UseCase.SystemGenerationConf.GetList;

namespace Interactor.SystemGenerationConf
{
    public class GetSystemGenerationConfListInteractor : IGetSystemGenerationConfListInputPort
    {
        private readonly ISystemGenerationConfRepository _systemGenerationConfRepository;

        public GetSystemGenerationConfListInteractor(ISystemGenerationConfRepository systemGenerationConfRepository)
        {
            _systemGenerationConfRepository = systemGenerationConfRepository;
        }

        public GetSystemGenerationConfListOutputData Handle(GetSystemGenerationConfListInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0) return new GetSystemGenerationConfListOutputData(new(), GetSystemGenerationConfListStatus.InvalidHpId);

                var result = _systemGenerationConfRepository.GetList(inputData.HpId);
                return new GetSystemGenerationConfListOutputData(result, GetSystemGenerationConfListStatus.Successed);
            }
            finally
            {
                _systemGenerationConfRepository.ReleaseResource();
            }
        }
    }
}