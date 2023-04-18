using Domain.Models.UserConf;
using UseCase.UserConf.UserSettingParam;

namespace Interactor.UserConf
{
    public class GetUserConfigParamInteractor : IGetUserConfigParamInputPort
    {
        private readonly IUserConfRepository _userConfRepository;

        public GetUserConfigParamInteractor(IUserConfRepository userConfRepository)
        {
            _userConfRepository = userConfRepository;
        }

        public GetUserConfigParamOutputData Handle(GetUserConfigParamInputData inputData)
        {
            if (inputData.HpId <= 0 || inputData.UserId <= 0) return new GetUserConfigParamOutputData(string.Empty, GetUserConfigParamStatus.NoData);

            var param = _userConfRepository.GetSettingParam(inputData.HpId, inputData.UserId, inputData.GrpCd, inputData.GrpItemCd);

            return new GetUserConfigParamOutputData(param, GetUserConfigParamStatus.Successed);

        }
    }
}
