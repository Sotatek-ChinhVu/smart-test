using Domain.Common;
using System.Collections;

namespace Domain.Models.SystemConf;

public interface ISystemConfRepository : IRepositoryBase
{
    List<SystemConfModel> GetList(int fromGrpCd, int toGrpCd);

    SystemConfModel GetByGrpCd(int hpId, int grpCd, int grpEdaNo);

    double GetSettingValue(int groupCd, int grpEdaNo, int hpId);

    string GetSettingParams(int groupCd, int grpEdaNo, int hpId, string defaultParam = "");

    Hashtable GetConfigForPrintFunction(int hpId);
}
