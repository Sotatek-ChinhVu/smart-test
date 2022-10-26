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
        InvalidFaild = 18,
        InvalidHpIdNotExist = 19,
        InvalidJihiSelectedHokenInfHokenNoEquals0 = 20,
        InvalidEmptyHoken = 21,
        InvalidHokenNashiOnly = 22,
        InvalidTokkiValue1 = 23,
        InvalidTokkiValue21 = 24,
        InvalidTokkiValue31 = 25,
        InvalidTokkiValue41 = 26,
        InvalidTokkiValue51 = 27,
        InvalidTokkiValue2 = 28,
        InvalidTokkiValue23 = 29,
        InvalidTokkiValue24 = 30,
        InvalidTokkiValue25 = 31,
        InvalidTokkiValue3 = 32,
        InvalidTokkiValue34 = 33,
        InvalidTokkiValue35 = 34,
        InvalidTokkiValue4 = 35,
        InvalidTokkiValue45 = 36,
        InvalidTokkiValue5 = 37,
        InvalidYukoKigen = 38,
        InvalidHokenSyaNoNullAndHokenNoNotEquals0 = 39,
        InvalidHokenNoEquals0 = 40,
        InvalidHokenNoHaveHokenMst = 41,
        InvalidHoubetu = 42,
        InvalidCheckDigitEquals1 = 43,
        InvalidCheckAgeHokenMst = 44,
        InvalidHokensyaNoNull = 45,
        InvalidHokensyaNoEquals0 = 46,
        InvalidHokensyaNoLength8StartWith39 = 47,
        InvalidKigoNull = 48,
        InvalidBangoNull = 49,
        InvalidHokenKbnEquals0 = 50,
        InvalidTokkurei = 51,
        InvalidConfirmDateAgeCheck = 52,
        InvalidHokenMstStartDate = 53,
        InvalidHokenMstEndDate = 54
    }
}
