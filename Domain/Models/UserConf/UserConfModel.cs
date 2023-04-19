﻿using static Helper.Constants.UserConfConst;

namespace Domain.Models.UserConf;

public class UserConfModel
{
    public UserConfModel(int userId, int grpCd, int grpItemCd, int grpItemEdaNo, int val, string param)
    {
        UserId = userId;
        GrpCd = grpCd;
        GrpItemCd = grpItemCd;
        GrpItemEdaNo = grpItemEdaNo;
        Val = val;
        Param = param;
    }

    public int UserId { get; private set; }

    public int GrpCd { get; private set; }

    public int GrpItemCd { get; private set; }

    public int GrpItemEdaNo { get; private set; }

    public int Val { get; private set; }

    public string Param { get; private set; }

    public UserConfStatus Validation()
    {
        if (GrpCd < 0)
        {
            return UserConfStatus.InvalidGrpCd;
        }
        if (GrpItemCd < 0)
        {
            return UserConfStatus.InvalidGrpItemCd;
        }
        if (GrpItemEdaNo < 0)
        {
            return UserConfStatus.InvalidGrpItemEdaNo;
        }
        if (Val < 0)
        {
            return UserConfStatus.InvalidVal;
        }
        if (Param.Length > 300)
        {
            return UserConfStatus.InvalidParam;
        }

        return UserConfStatus.Valid;
    }
    public UserConfModel ChangeParam(string param)
    {
        Param = param;
        return this;
    }
}