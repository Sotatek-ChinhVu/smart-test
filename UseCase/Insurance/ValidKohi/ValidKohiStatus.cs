using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.Insurance.ValidKohi
{
    public enum ValidKohiStatus: byte
    {
        ValidSuccess = 0,
        InvalidSindate = 1,
        InvalidPtBirthday = 2,
        InvalidSelectedKohiStartDate1 = 3,
        InvalidSelectedKohiEndDate1 = 4,
        InvalidSelectedKohiConfirmDate1 = 5,
        InvalidSelectedKohiHokenNo1 = 6,
        InvalidSelectedKohiMstFutansyaCheckFlag1 = 7,
        InvalidSelectedKohiMstJyukyusyaCheckFlag1 = 8,
        InvalidSelectedKohiMstJyuKyuCheckDigit1 = 9,
        InvalidSelectedKohiMst1TokusyuCheckFlag1 = 10,
        InvalidSelectedKohiMstStartDate1 = 11,
        InvalidSelectedKohiMstEndDate1 = 12,
        InvalidSelectedKohiMstCheckDigit = 13,
        InvalidSelectedKohiMstAgeStart1 = 14,
        InvalidSelectedKohiMstAgeEnd1 = 15
    }
}
