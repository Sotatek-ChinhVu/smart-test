using Domain.Common;
using System.Collections;

namespace Domain.Models.SystemConf;

public interface ISystemConfRepository : IRepositoryBase
{
    List<SystemConfModel> GetList(int hpId, int fromGrpCd, int toGrpCd);

    List<SystemConfModel> GetList(int hpId, List<int> grpCodeList);

    SystemConfModel GetByGrpCd(int hpId, int grpCd, int grpEdaNo);

    List<SystemConfModel> GetAllSystemConfig(int hpId);

    double GetSettingValue(int groupCd, int grpEdaNo, int hpId);

    string GetSettingParams(int groupCd, int grpEdaNo, int hpId, string defaultParam = "");

    Hashtable GetConfigForPrintFunction(int hpId);

    bool SaveSystemConfigList(int hpId, int userId, List<SystemConfModel> systemConfigList);

    List<SystemConfMenuModel> GetListSystemConfMenuWithGeneration(int hpId, List<int> menuGrp);

    //Key: RoudouCd, Value: RoudouName
    Dictionary<string, string> GetRoudouMst(int hpId);

    List<SystemConfMenuModel> GetListSystemConfMenu(int hpId, List<int> menuGrp);

    List<SystemConfMenuModel> GetListSystemConfMenuOnly(int hpId, int menuGrp);

    List<string> GetListCenterCd(int hpId);

    bool SaveSystemGenerationConf(int userId, List<SystemConfMenuModel> systemConfMenuModels);

    bool SaveSystemSetting(int hpId, int userId, List<SystemConfMenuModel> SystemConfMenuModels);

    List<SystemConfListXmlPathModel> GetSystemConfListXmlPath(int hpId, int grpCd, string machine, bool isKensaIrai);

    List<SystemConfListXmlPathModel> GetAllPathConf(int hpId);

    bool SavePathConfOnline(int hpId, int userId, List<SystemConfListXmlPathModel> systemConfListXmlPathModels);
}
