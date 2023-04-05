﻿using Domain.Common;
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

    List<SystemConfMenuModel> GetListSystemConfMenuWithGeneration(int hpId, int menuGrp);

    Dictionary<string, string> GetRoudouMst();

    List<HpInfModel> GetListHpInf(int hpId);
}
