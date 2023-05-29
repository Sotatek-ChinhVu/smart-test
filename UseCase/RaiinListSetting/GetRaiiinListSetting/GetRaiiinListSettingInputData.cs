using UseCase.Core.Sync.Core;

namespace UseCase.RaiinListSetting.GetRaiiinListSetting
{
    public class GetRaiiinListSettingInputData : IInputData<GetRaiiinListSettingOutputData>
    {
        public GetRaiiinListSettingInputData(int hpId)
        {
            HpId = hpId;
        }

        public int HpId { get; private set; }
    }
}
