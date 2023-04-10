using UseCase.Core.Sync.Core;

namespace UseCase.SystemConf.SystemSetting
{
    public class GetSystemSettingInputData : IInputData<GetSystemSettingOutputData>
    {
        public GetSystemSettingInputData(int hpId)
        {
            HpId = hpId;
        }

        public int HpId { get; private set; }
    }
}
