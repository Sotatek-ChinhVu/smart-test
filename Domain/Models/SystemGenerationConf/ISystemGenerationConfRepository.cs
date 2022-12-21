using Domain.Common;

namespace Domain.Models.SystemGenerationConf
{
    public interface ISystemGenerationConfRepository : IRepositoryBase
    {
        (int, string) GetSettingValue(int hpId, int groupCd, int grpEdaNo = 0, int presentDate = 0, int defaultValue = 0, string defaultParam = "", bool fromLastestDb = false);
    }
}
