using UseCase.Core.Sync.Core;

namespace UseCase.TimeZoneConf.GetTimeZoneConfGroup
{
    public class GetTimeZoneConfGroupInputData : IInputData<GetTimeZoneConfGroupOutputData>
    {
        public GetTimeZoneConfGroupInputData(int hpId, int userId)
        {
            HpId = hpId;
            UserId = userId;
        }

        public int HpId { get; private set; }

        public int UserId { get; private set; }
    }
}
