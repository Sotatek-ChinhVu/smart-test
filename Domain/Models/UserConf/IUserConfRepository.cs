﻿using Domain.Common;

namespace Domain.Models.UserConf;

public interface IUserConfRepository : IRepositoryBase
{
    List<UserConfModel> GetList(int userId, int fromGrpCd, int toGrpCd);

    List<UserConfModel> GetList(int hpId, int userId, List<int> grpCodes);

    Dictionary<string, int> GetList(int userId);

    List<UserConfModel> GetListUserConf(int hpId, int userId, int groupCd);

    void UpdateAdoptedByomeiConfig(int hpId, int userId, int adoptedValue);

    void UpdateUserConf(int hpId, int userId, int grpCd, int value);

    int Sagaku(bool fromRece);

    int GetDefaultValue(int groupCd, int groupItemCd = 0);

    int GetSettingValue(int hpId, int userId, int groupCd, int grpItemCd = 0, int grpItemEdaNo = 0);

    string GetSettingParam(int hpId, int userId, int groupCd, int grpItemCd = 0, string defaultValue = "");

    bool UpsertUserConfs(int hpId, int userId, List<UserConfModel> userConfs);
}