using Domain.Common;
using Domain.Models.HpInf;
using System.Collections;

namespace Domain.Models.SystemConf;

public interface ISystemConfRepository : IRepositoryBase
{
    List<SystemConfModel> GetList(int fromGrpCd, int toGrpCd);

    SystemConfModel GetByGrpCd(int hpId, int grpCd, int grpEdaNo);

    List<SystemConfModel> GetAllSystemConfig(int hpId);

    double GetSettingValue(int groupCd, int grpEdaNo, int hpId);

    string GetSettingParams(int groupCd, int grpEdaNo, int hpId, string defaultParam = "");

    Hashtable GetConfigForPrintFunction(int hpId);

    List<SystemConfMenuModel> GetListSystemConfMenuWithGeneration(int hpId, List<int> menuGrp);

    //Key: RoudouCd, Value: RoudouName
    Dictionary<string, string> GetRoudouMst();

    List<SystemConfMenuModel> GetListSystemConfMenu(int hpId, List<int> menuGrp);

    List<SystemConfMenuModel> GetListSystemConfMenuOnly(int hpId, int menuGrp);

    List<string> GetListCenterCd(int hpId);
}
