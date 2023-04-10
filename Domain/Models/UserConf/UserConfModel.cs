using Helper.Extension;
using static Helper.Constants.UserConfConst;

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

    public UserConfModel(string commentCheckSaveParam, string inputCheckSaveParam, string santeiCheckSaveParam)
    {
        CommentCheckSaveParam = commentCheckSaveParam;
        InputCheckSaveParam = inputCheckSaveParam;
        SanteiCheckSaveParam = santeiCheckSaveParam;
    }

    public int UserId { get; private set; }

    public int GrpCd { get; private set; }

    public int GrpItemCd { get; private set; }

    public int GrpItemEdaNo { get; private set; }

    public int Val { get; private set; }

    public string Param { get; private set; }

    public string CommentCheckSaveParam { get; private set; }

    public string InputCheckSaveParam { get; private set; }

    public string SanteiCheckSaveParam { get; private set; }

    public bool IsCmtCheckPrint => CommentCheckSaveParam[4].AsInteger() == 1;

    public bool IsInputCheckPrint => InputCheckSaveParam[4].AsInteger() == 1;

    public bool IsSanteiCheckPrint => SanteiCheckSaveParam[4].AsInteger() == 1;

    public bool IsCmtCheckTrialCalc => CommentCheckSaveParam[3].AsInteger() == 1;

    public bool IsInputCheckTrialCalc => InputCheckSaveParam[3].AsInteger() == 1;

    public bool IsSanteiCheckTrialCalc => SanteiCheckSaveParam[3].AsInteger() == 1;

    public bool ShowSaveConfirmWithPrint
    {
        get
        {
            return IsCmtCheckPrint ||
                   IsInputCheckPrint ||
                   IsSanteiCheckPrint;
        }
    }

    public bool ShowSaveConfirmWithTrialCalc
    {
        get
        {
            return IsCmtCheckTrialCalc ||
                   IsInputCheckTrialCalc ||
                   IsSanteiCheckTrialCalc;
        }
    }

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
}