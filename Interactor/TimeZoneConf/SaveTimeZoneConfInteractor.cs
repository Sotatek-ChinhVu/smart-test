using Domain.Models.TimeZone;
using Domain.Models.User;
using Helper.Constants;
using static Helper.Constants.UserConst;
using UseCase.TimeZoneConf.SaveTimeZoneConf;

namespace Interactor.TimeZoneConf
{
    public class SaveTimeZoneConfInteractor : ISaveTimeZoneConfInputPort
    {
        private readonly ITimeZoneRepository _timeZoneConfRepository;
        private readonly IUserRepository _userRepository;

        public SaveTimeZoneConfInteractor(ITimeZoneRepository timeZoneConfRepository, IUserRepository userRepository)
        {
            _timeZoneConfRepository = timeZoneConfRepository;
            _userRepository = userRepository;
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
            finally
            {
                _timeZoneConfRepository.ReleaseResource();
                _userRepository.ReleaseResource();
            }
        }
    }
}
