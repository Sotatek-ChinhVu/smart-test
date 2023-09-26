using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.MstItem
{
    public enum ActionCompareSearchModel
    {
        All, // 13~89
        Instruction, // 13
        Prescription, // 14
        Treatment, // 20~29
        Inspection, // 30~39
        Other, // 40~49
        AtHome, // 50~59
        Injection, //60~69
        Surgery, // 70~79
        Image, //80~89
    }

    public enum ComparisonSearchModel
    {
        Name,
        ReceName,
        OdrUnitName,
        ReceUnitName,
        SaiketuKbn,
        CmtKbn
    }
}
