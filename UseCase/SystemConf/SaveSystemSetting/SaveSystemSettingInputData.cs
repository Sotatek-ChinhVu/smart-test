using Domain.Models.HpInf;
using UseCase.Core.Sync.Core;

namespace UseCase.SystemConf.SaveSystemSetting
{
    public class SaveSystemSettingInputData : IInputData<SaveSystemSettingOutputData>
    {
        public SaveSystemSettingInputData(int userId, List<HpInfModel> hpInfs, bool isUpdateHpInfo)
        {
            UserId = userId;
            HpInfs = hpInfs;
            IsUpdateHpInfo = isUpdateHpInfo;
        }

        public int UserId { get; private set; }
        public List<HpInfModel> HpInfs { get; private set; }
        public bool IsUpdateHpInfo { get; private set; }
    }
}
