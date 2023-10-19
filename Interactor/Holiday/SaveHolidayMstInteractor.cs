using Domain.Models.FlowSheet;
using Domain.Models.User;
using Helper.Constants;
using UseCase.Holiday.SaveHoliday;
using static Helper.Constants.UserConst;

namespace Interactor.Holiday
{
    public class SaveHolidayMstInteractor : ISaveHolidayMstInputPort
    {
        private readonly IFlowSheetRepository _flowSheetRepository;
        private readonly IUserRepository _userRepository;

        public SaveHolidayMstInteractor(IFlowSheetRepository flowSheetRepository, IUserRepository userRepository)
        {
            _flowSheetRepository = flowSheetRepository;
            _userRepository = userRepository;
        }

        public SaveHolidayMstOutputData Handle(SaveHolidayMstInputData inputData)
        {
            if (inputData.UserId <= 0)
                return new SaveHolidayMstOutputData(SaveHolidayMstStatus.InvalidUserId);
            try
            {
                if (_userRepository.GetPermissionByScreenCode(inputData.Holiday.HpId, inputData.UserId, FunctionCode.HolidaySettingCode) != PermissionType.Unlimited)
                {
                    return new SaveHolidayMstOutputData(SaveHolidayMstStatus.NoPermission);
                }
                bool result = _flowSheetRepository.SaveHolidayMst(inputData.Holiday, inputData.UserId);
                if (result)
                    return new SaveHolidayMstOutputData(SaveHolidayMstStatus.Successful);
                else
                    return new SaveHolidayMstOutputData(SaveHolidayMstStatus.Failed);
            }
            finally
            {
                _flowSheetRepository.ReleaseResource();
                _userRepository.ReleaseResource();
            }
        }
    }
}
