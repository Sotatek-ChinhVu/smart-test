using Domain.Models.SystemConf;
using UseCase.SystemConf;

namespace Interactor.SystemConf
{
    public class GetSystemConfInteractor : IGetSystemConfInputPort
    {
        private readonly ISystemConfRepository _systemConfRepository;

        public GetSystemConfInteractor(ISystemConfRepository systemConfRepository)
        {
            _systemConfRepository = systemConfRepository;
        }

        public GetSystemConfOutputData Handle(GetSystemConfInputData inputData)
        {
            if (inputData.HpId <= 0) return new GetSystemConfOutputData(GetSystemConfStatus.InvalidHpId);
            if (inputData.GrpCd <= 0) return new GetSystemConfOutputData(GetSystemConfStatus.InvalidGrpCd);
            var result = _systemConfRepository.GetByGrpCd(inputData.HpId, inputData.GrpCd, inputData.GrpEdaNo);
            return new GetSystemConfOutputData(result, GetSystemConfStatus.Successed);
        }
    }
}
