using Domain.Models.TimeZone;
using UseCase.Core.Sync.Core;

namespace UseCase.TimeZoneConf.SaveTimeZoneConf
{
    public class SaveTimeZoneConfInputData : IInputData<SaveTimeZoneConfOutputData>
    {
        public SaveTimeZoneConfInputData(int hpId, int userId, List<TimeZoneConfModel> timeZoneConfs)
        {
            HpId = hpId;
            UserId = userId;
            TimeZoneConfs = timeZoneConfs;
        }

        public int HpId { get; private set; }

        public int UserId { get; private set; }

        public List<TimeZoneConfModel> TimeZoneConfs { get; private set; }
    }
}
