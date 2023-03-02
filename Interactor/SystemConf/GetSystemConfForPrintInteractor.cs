using Domain.Models.SystemConf;
using UseCase.SystemConf.GetSystemConfForPrint;

namespace Interactor.SystemConf
{
    public class GetSystemConfForPrintInteractor : IGetSystemConfForPrintInputPort
    {
        private readonly ISystemConfRepository _systemConfRepository;

        public GetSystemConfForPrintInteractor(ISystemConfRepository systemConfRepository)
        {
            _systemConfRepository = systemConfRepository;
        }

        public GetSystemConfForPrintOutputData Handle(GetSystemConfForPrintInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0) return new GetSystemConfForPrintOutputData(new(), GetSystemConfForPrintStatus.InvalidHpId);

                var result = _systemConfRepository.GetConfigForPrintFunction(inputData.HpId);
                return new GetSystemConfForPrintOutputData(result, GetSystemConfForPrintStatus.Successed);
            }
            finally
            {
                _systemConfRepository.ReleaseResource();
            }
        }
    }
}
