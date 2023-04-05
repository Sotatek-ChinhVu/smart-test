﻿using Domain.Common;
using System.Collections;

namespace Domain.Models.SystemConf;

public interface ISystemConfRepository : IRepositoryBase
{
    List<SystemConfModel> GetList(int fromGrpCd, int toGrpCd);

    List<SystemConfModel> GetList(int hpId, List<int> grpCodeList);

    SystemConfModel GetByGrpCd(int hpId, int grpCd, int grpEdaNo);

    List<SystemConfModel> GetAllSystemConfig(int hpId);

    double GetSettingValue(int groupCd, int grpEdaNo, int hpId);

    string GetSettingParams(int groupCd, int grpEdaNo, int hpId, string defaultParam = "");

    Hashtable GetConfigForPrintFunction(int hpId);
}
