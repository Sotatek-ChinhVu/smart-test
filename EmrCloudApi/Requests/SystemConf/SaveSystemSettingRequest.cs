﻿using UseCase.SystemConf.SaveSystemSetting;

namespace EmrCloudApi.Requests.SystemConf
{
    public class SaveSystemSettingRequest
    {
        public List<HpInfItem> HpInfs { get; set; } = new();
        public List<SystemConfMenuItem> SystemConfMenus { get; set; } = new();
        public List<SanteiInfDetailItem> SanteiInfs { get; set; } = new();
        public List<KensaCenterMstItem> KensaCenters { get; set; } = new();
    }
}
