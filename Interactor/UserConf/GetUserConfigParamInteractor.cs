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
            try
            {
                if (inputData.HpId <= 0 || inputData.UserId <= 0) return new GetUserConfigParamOutputData(new(), GetUserConfigParamStatus.NoData);

                var param = _userConfRepository.GetListSettingParam(inputData.HpId, inputData.UserId, inputData.GroupCode);

                return new GetUserConfigParamOutputData(param, GetUserConfigParamStatus.Successed);
            }
            finally
            {
                _userConfRepository.ReleaseResource();
            }

        }
    }
}
