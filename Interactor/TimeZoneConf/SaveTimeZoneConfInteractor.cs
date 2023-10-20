using Domain.Models.TimeZone;
using Domain.Models.User;
using Helper.Constants;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using UseCase.TimeZoneConf.SaveTimeZoneConf;
using static Helper.Constants.UserConst;

namespace Interactor.TimeZoneConf
{
    public class SaveTimeZoneConfInteractor : ISaveTimeZoneConfInputPort
    {
        private readonly ITimeZoneRepository _timeZoneConfRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILoggingHandler _loggingHandler;
        private readonly ITenantProvider _tenantProvider;

        public SaveTimeZoneConfInteractor(ITenantProvider tenantProvider, ITimeZoneRepository timeZoneConfRepository, IUserRepository userRepository)
        {
            _timeZoneConfRepository = timeZoneConfRepository;
            _userRepository = userRepository;
            _tenantProvider = tenantProvider;
            _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
        }

        public SaveTimeZoneConfOutputData Handle(SaveTimeZoneConfInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0) return new SaveTimeZoneConfOutputData(SaveTimeZoneConfStatus.InvalidHpId);

                if (inputData.UserId <= 0) return new SaveTimeZoneConfOutputData(SaveTimeZoneConfStatus.InvalidUserId);

                if (!inputData.TimeZoneConfs.Any()) return new SaveTimeZoneConfOutputData(SaveTimeZoneConfStatus.NoData);

                bool isHavePermission = _userRepository.GetPermissionByScreenCode(inputData.HpId, inputData.UserId, FunctionCode.MasterMaintenanceCode) == PermissionType.Unlimited;

                if (!isHavePermission) return new SaveTimeZoneConfOutputData(SaveTimeZoneConfStatus.NotHavePermission);

                bool result = _timeZoneConfRepository.SaveTimeZoneConf(inputData.HpId, inputData.UserId, inputData.TimeZoneConfs);

                if (result) return new SaveTimeZoneConfOutputData(SaveTimeZoneConfStatus.Successful);
                else return new SaveTimeZoneConfOutputData(SaveTimeZoneConfStatus.Failed);
            }
            catch (Exception ex)
            {
                _loggingHandler.WriteLogExceptionAsync(ex);
                throw;
            }
            finally
            {
                _timeZoneConfRepository.ReleaseResource();
                _userRepository.ReleaseResource();
                _loggingHandler.Dispose();
            }
        }
    }
}
