using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.Insurance.ValidMainInsurance
{
    public enum ValidMainInsuranceStatus: byte
    {
        ValidSuccess = 0,
        InvalidHpId = 1,
        InvalidSinDate = 2,
        InvalidPtBirthday = 3,
        InvalidSelectedHokenInfHokenNo = 4,
        InvalidSelectedHokenInfIsAddNew = 5,
        InvalidSelectedHokenInfStartDate = 6,
        InvalidSelectedHokenInfEndDate = 7,
        InvaliSelectedHokenInfHokensyaMstIsKigoNa = 8,
        InvalidSelectedHokenInfHonkeKbn = 9,
        InvalidSelectedHokenInfTokureiYm1 = 10,
        InvalidSelectedHokenInfTokureiYm2 = 11,
        InvalidSelectedHokenInfConfirmDate = 12,
        InvalidSelectedHokenMstHokenNo = 13,
        InvalidSelectedHokenMstCheckDegit = 14,
        InvalidSelectedHokenMstAgeStart = 15,
        InvalidSelectedHokenMstAgeEnd = 16,
        InvalidSelectedHokenMstStartDate = 17,
        InvalidSelectedHokenMstEndDate = 18,
        InvalidFaild = 19

    }
}
