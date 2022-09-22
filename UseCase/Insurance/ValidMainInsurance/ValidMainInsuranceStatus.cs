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
        InvalidSelectedHokenInfStartDate = 5,
        InvalidSelectedHokenInfEndDate = 6,
        InvaliSelectedHokenInfHokensyaMstIsKigoNa = 7,
        InvalidSelectedHokenInfHonkeKbn = 8,
        InvalidSelectedHokenInfTokureiYm1 = 9,
        InvalidSelectedHokenInfTokureiYm2 = 10,
        InvalidSelectedHokenInfConfirmDate = 11,
        InvalidSelectedHokenMstHokenNo = 12,
        InvalidSelectedHokenMstCheckDegit = 13,
        InvalidSelectedHokenMstAgeStart = 14,
        InvalidSelectedHokenMstAgeEnd = 15,
        InvalidSelectedHokenMstStartDate = 16,
        InvalidSelectedHokenMstEndDate = 17,
        InvalidFaild = 18
    }
}
