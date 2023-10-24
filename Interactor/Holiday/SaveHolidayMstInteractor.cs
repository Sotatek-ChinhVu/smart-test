using Domain.Models.FlowSheet;
using Domain.Models.User;
using Helper.Constants;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using UseCase.Holiday.SaveHoliday;
using static Helper.Constants.UserConst;

namespace Interactor.Holiday
{
    public class SaveHolidayMstInteractor : ISaveHolidayMstInputPort
    {
        private readonly IFlowSheetRepository _flowSheetRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILoggingHandler _loggingHandler;
        private readonly ITenantProvider _tenantProvider;

        public SaveHolidayMstInteractor(ITenantProvider tenantProvider, IFlowSheetRepository flowSheetRepository, IUserRepository userRepository)
        {
            _flowSheetRepository = flowSheetRepository;
            _userRepository = userRepository;
            _tenantProvider = tenantProvider;
            _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
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
            catch (Exception ex)
            {
                _loggingHandler.WriteLogExceptionAsync(ex);
                throw;
            }
            finally
            {
                _flowSheetRepository.ReleaseResource();
                _userRepository.ReleaseResource();
                _loggingHandler.Dispose();
            }
        }
    }
}
