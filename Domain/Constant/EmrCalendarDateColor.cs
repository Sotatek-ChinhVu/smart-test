using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Constant
{
    public enum EmrCalendarDateColor
    {
        // Background
        [Description("#FFFFFF")]
        Default,//0
        [Description("#FFCC3B")]
        HasFirstVisit,// 1,6
        [Description("#FFA8A8")]
        HasOwnExpense,// 5
        [Description("#9FDBEE")]
        CalHasCallAgain,// 3,4,7,8
        [Description("#DEDEDE")]
        CalHasData,// 0
        [Description("#DEDEDE")]
        CalNextOrder,// 0
        [Description("#FFD2FD")]
        Holiday,
        [Description("#FFD5B9")]
        HolidayRed,
        [Description("#008000")]
        Today,
        [Description("#FFD2FD")]
        Sunday,
        [Description("#CAFDFB")]
        Saturday,
        // ForeGround
        [Description("#FF0000")]
        RedForeGround,
        [Description("#0000FF")]
        BlueForeGround,
        [Description("#000000")]
        NormalForeGround,
        [Description("#FFCAFF")]
        CurrentMonthBackground,
        [Description("#E4F0F5")]
        NormalMonthBackground,
        //Border
        [Description("#FF0000")]
        RedBorder,
    }
}
