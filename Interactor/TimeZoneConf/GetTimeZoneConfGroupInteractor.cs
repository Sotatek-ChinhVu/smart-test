using Domain.Models.TimeZoneConf;
using Domain.Models.User;
using Helper.Constants;
using UseCase.TimeZoneConf.GetTimeZoneConfGroup;
using static Helper.Constants.UserConst;

namespace Interactor.TimeZoneConf
{
    public class GetTimeZoneConfGroupInteractor : IGetTimeZoneConfGroupInputPort
    {
        private readonly ITimeZoneConfRepository _timeZoneConfRepository;
        private readonly IUserRepository _userRepository;

        public GetTimeZoneConfGroupInteractor(ITimeZoneConfRepository timeZoneConfRepository, IUserRepository userRepository)
        {
            _timeZoneConfRepository = timeZoneConfRepository;
            _userRepository = userRepository;
        }

        public GetTimeZoneConfGroupOutputData Handle(GetTimeZoneConfGroupInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0) return new GetTimeZoneConfGroupOutputData(GetTimeZoneConfGroupStatus.InvalidHpId, false, new List<TimeZoneConfGroupModel>());

                List<TimeZoneConfGroupModel> datas = _timeZoneConfRepository.GetTimeZoneConfGroupModels(inputData.HpId).OrderBy(u => u.YoubiKbn).ToList();
                if (!datas.Any())
                    return new GetTimeZoneConfGroupOutputData(GetTimeZoneConfGroupStatus.NoData, false, datas);
                else
                {
                    int defaultYouKbn = (int)DateTime.Now.DayOfWeek + 1;
                    var timeZoneConfGroupModel = datas.Where(x => x.YoubiKbn == defaultYouKbn).FirstOrDefault();
                    if (timeZoneConfGroupModel != null && timeZoneConfGroupModel.Details.Count() == 1 &&
                        timeZoneConfGroupModel.Details[0].StartTime == 0 &&
                        timeZoneConfGroupModel.Details[0].EndTime == 0)
                    {
                        timeZoneConfGroupModel.Details[0].SetIsNewStartTime(true);
                        timeZoneConfGroupModel.Details[0].SetIsNewEndTime(true);
                    }

                    bool isHavePermission = _userRepository.GetPermissionByScreenCode(inputData.HpId, inputData.UserId ,FunctionCode.MasterMaintenanceCode) == PermissionType.Unlimited;
                    return new GetTimeZoneConfGroupOutputData(GetTimeZoneConfGroupStatus.Successful, isHavePermission, datas);
                }    
            }
            finally
            {
                _timeZoneConfRepository.ReleaseResource();
                _userRepository.ReleaseResource();
            }
        }
    }
}
