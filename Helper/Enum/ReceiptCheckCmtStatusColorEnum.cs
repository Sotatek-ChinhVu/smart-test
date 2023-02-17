using System.ComponentModel;

namespace Helper.Enum;

public enum ReceiptCheckCmtStatusColorEnum : byte
{
    [Description("#00A2E8")]
    Normal,
    [Description("#FFFFFF")]
    Uncomfirmed,
    [Description("#FFC000")]
    SystemHold,
    [Description("#C0514E")]
    Keep1,
    [Description("#F79647")]
    Keep2,
    [Description("#8064A2")]
    Keep3,
    [Description("#9BBB59")]
    TempComfofirm,
    [Description("#4F81BD")]
    Confirmed,
}
