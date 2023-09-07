using Domain.Models.SystemConf;
using UseCase.SystemConf.GetPathAll;

namespace Interactor.SystemConf
{
    public class GetAllPathInteractor : IGetPathAllInputPort
    {
        private readonly ISystemConfRepository _systemConfRepository;

        public GetAllPathInteractor(ISystemConfRepository systemConfRepository)
        {
            _systemConfRepository = systemConfRepository;
        }

        public GetPathAllOutputData Handle(GetPathAllInputData inputData)
        {
            try
            {
                var result = _systemConfRepository.GetAllPathConf(inputData.HpId);
                if (result.Count > 0)
                {
                    return new GetPathAllOutputData(result, GetPathAllStatus.Successed);
                }

                return new GetPathAllOutputData(result, GetPathAllStatus.NoData);
            }
            finally
            {
                _systemConfRepository.ReleaseResource();
            }
        }
    }
}
