namespace Domain.Models.SystemGenerationConf
{
    public interface ISystemGenerationConfRepository
    {
        int GetSettingValue(int hpId, int groupCd, int grpEdaNo = 0, int presentDate = 0, int defaultValue = 0, bool fromLastestDb = false);

        public int RefillSetting(int presentDate);
    }
}
