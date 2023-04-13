using Helper.Constants;

namespace UseCase.SystemConf.SaveSystemSetting
{
    public class SystemConfigItem
    {
        public SystemConfigItem(int hpId, int grpCd, int grpEdaNo, double val, string param, string biko, bool isUpdatePtRyosyo, ModelStatus systemSettingModelStatus)
        {
            HpId = hpId;
            GrpCd = grpCd;
            GrpEdaNo = grpEdaNo;
            Val = val;
            Param = param;
            Biko = biko;
            IsUpdatePtRyosyo = isUpdatePtRyosyo;
            SystemSettingModelStatus = systemSettingModelStatus;
        }

        public int HpId { get; private set; }

        public int GrpCd { get; private set; }

        public int GrpEdaNo { get; private set; }

        public double Val { get; private set; }

        public string Param { get; private set; }

        public string Biko { get; private set; }

        public bool IsUpdatePtRyosyo { get; private set; }

        public ModelStatus SystemSettingModelStatus { get; private set; }
    }
}
