﻿using Domain.Common;

namespace Domain.Models.UserConf;

public interface IUserConfRepository : IRepositoryBase
{
    List<UserConfModel> GetList(int userId, int fromGrpCd, int toGrpCd);

    List<UserConfModel> GetList(int hpId, int userId, List<int> grpCodes);

    Dictionary<string, int> GetDic(int hpId, int userId);

    List<UserConfModel> GetListUserConf(int hpId, int userId, int groupCd);

    void UpdateAdoptedByomeiConfig(int hpId, int userId, int adoptedValue);

    void UpdateUserConf(int hpId, int userId, int grpCd, int value);

    int Sagaku(int hpId, int userId, bool fromRece);

    int GetDefaultValue(int groupCd, int groupItemCd = 0);

    int GetSettingValue(int hpId, int userId, int groupCd, int grpItemCd = 0, int grpItemEdaNo = 0);

    List<(int groupItemCd, int value)> GetSettingValues(int hpId, int userId, int groupCd, int fromGroupItemCd, int toGroupItemCd);

    string GetSettingParam(int hpId, int userId, int groupCd, int grpItemCd = 0, string defaultValue = "");

    bool UpsertUserConfs(int hpId, int userId, List<UserConfModel> userConfs);
    List<UserConfModel> GetListSettingParam(int hpId, int userId, List<Tuple<int, int>> groupCode, string defaultValue = "");

    List<UserConfModel> GetList(int hpId, int userId);
}