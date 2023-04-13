namespace UseCase.SystemConf.SaveSystemSetting
{
    public class SystemConfMenuItem
    {
        public SystemConfMenuItem(List<SystemGenerationConfItem> systemGenerationConfs, SystemConfigItem systemConf)
        {
            SystemGenerationConfs = systemGenerationConfs;
            SystemConf = systemConf;
        }

        public List<SystemGenerationConfItem> SystemGenerationConfs { get; private set; }
        public SystemConfigItem SystemConf { get; private set; }
    }
}
