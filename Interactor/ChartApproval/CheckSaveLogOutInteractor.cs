using Domain.Models.ChartApproval;
using Domain.Models.SystemConf;
using Domain.Models.User;
using Helper.Constants;
using Helper.Extension;
using UseCase.ChartApproval.CheckSaveLogOut;
using static Helper.Constants.UserConst;

namespace Interactor.ChartApproval
{
    public class CheckSaveLogOutInteractor : ICheckSaveLogOutInputPort
    {
        private readonly IApprovalInfRepository _approvalInfRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISystemConfRepository _systemConfRepository;

        public CheckSaveLogOutInteractor(IApprovalInfRepository approvalInfRepository, IUserRepository userRepository, ISystemConfRepository systemConfRepository)
        {
            _approvalInfRepository = approvalInfRepository;
            _userRepository = userRepository;
            _systemConfRepository = systemConfRepository;
        }

        public CheckSaveLogOutOutputData Handle(CheckSaveLogOutInputData input)
        {
            try
            {
                if (input.UserId < 0)
                    return new CheckSaveLogOutOutputData(CheckSaveLogOutStatus.InvalidUserId);

                if (input.HpId < 0)
                    return new CheckSaveLogOutOutputData(CheckSaveLogOutStatus.InvalidHpId);

                bool approvalFuncSetting = _systemConfRepository.GetSettingValue(2022, 0, input.HpId) == 1;
                PermissionType permission = _userRepository.GetPermissionByScreenCode(input.HpId, input.UserId, FunctionCode.ApprovalInfo);

                if (approvalFuncSetting && permission == PermissionType.Unlimited)
                {
                    var param = _systemConfRepository.GetSettingParams(2022, 0, input.HpId);

                    bool result = _approvalInfRepository.NeedApprovalInf(input.HpId ,param.AsInteger(), input.DepartmentId, input.UserId);
                    if (result)
                        return new CheckSaveLogOutOutputData(CheckSaveLogOutStatus.NeedSave);
                    else
                        return new CheckSaveLogOutOutputData(CheckSaveLogOutStatus.NoNeedSave);
                }
                else
                {
                    return new CheckSaveLogOutOutputData(CheckSaveLogOutStatus.NoNeedSave);
                }
            }
            finally
            {
                _approvalInfRepository.ReleaseResource();
                _userRepository.ReleaseResource();
                _systemConfRepository.ReleaseResource();
            }
        }
    }
}
