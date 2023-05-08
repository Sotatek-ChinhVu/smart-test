using Domain.Models.TimeZoneConf;
using UseCase.Core.Sync.Core;

namespace UseCase.TimeZoneConf.GetTimeZoneConfGroup
{
    public class GetTimeZoneConfGroupOutputData : IOutputData
    {
        public GetTimeZoneConfGroupOutputData(GetTimeZoneConfGroupStatus status, bool isHavePermission, List<TimeZoneConfGroupModel> timeZoneConfGroups)
        {
            Status = status;
            IsHavePermission = isHavePermission;
            TimeZoneConfGroups = timeZoneConfGroups;
        }

        public GetTimeZoneConfGroupStatus Status { get; private set; }

        public bool IsHavePermission { get; private set; }

        public List<TimeZoneConfGroupModel> TimeZoneConfGroups { get; private set; }
    }
}
