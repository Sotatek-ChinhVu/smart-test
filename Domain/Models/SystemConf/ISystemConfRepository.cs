using Domain.Common;

namespace Domain.Models.SystemConf;

public interface ISystemConfRepository : IRepositoryBase
{
    List<SystemConfModel> GetList(int fromGrpCd, int toGrpCd);

    SystemConfModel GetByGrpCd(int hpId, int grpCd, int grpEdaNo);

    List<SystemConfModel> GetListByGrpCd(int hpId, List<SystemConfModel> grpItemList);

    double GetSettingValue(int groupCd, int grpEdaNo, int hpId);

    string GetSettingParams(int groupCd, int grpEdaNo, int hpId, string defaultParam = "");
}
