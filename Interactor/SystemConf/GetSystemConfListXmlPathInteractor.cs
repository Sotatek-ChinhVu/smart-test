using Domain.Models.SystemConf;
using UseCase.SystemConf.GetXmlPath;

namespace Interactor.SystemConf
{
    public class GetSystemConfListXmlPathInteractor : IGetSystemConfListXmlPathInputPort
    {
        private readonly ISystemConfRepository _systemConfRepository;

        public GetSystemConfListXmlPathInteractor(ISystemConfRepository systemConfRepository)
        {
            _systemConfRepository = systemConfRepository;
        }

        public GetSystemConfListXmlPathOutputData Handle(GetSystemConfListXmlPathInputData inputData)
        {
            try
            {
                var result = _systemConfRepository.GetSystemConfXmlPath(inputData.HpId, inputData.GrpCd, inputData.Machine);
                return new GetSystemConfListXmlPathOutputData(result, GetSystemConfListXmlPathStatus.Successed);
            }
            finally
            {
                _systemConfRepository.ReleaseResource();
            }
        }
    }
}