using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.Insurance.ValidPatternExpirated
{
    public enum ValidPatternExpiratedStatus: byte
    {
        ValidPatternExpiratedSuccess = 0,
        InvalidHpId = 1,
        InvalidPtId = 2,
        InvalidSinDate = 3,
        InvalidPatternHokenPid = 4,
        InvalidPatternConfirmDate = 5,
        InvalidHokenInfStartDate = 6,
        InvalidHokenInfEndDate = 7,
        InvalidHokenMstStartDate = 8,
        InvalidHokenMstEndDate = 9,
        InvalidKohiConfirmDate1 = 10,
        InvalidKohiHokenMstStartDate1 = 11,
        InvalidKohiHokenMstEndDate1 = 12,
        InvalidKohiConfirmDate2 = 13,
        InvalidKohiHokenMstStartDate2 = 14,
        InvalidKohiHokenMstEndDate2 = 15,
        InvalidKohiConfirmDate3 = 16,
        InvalidKohiHokenMstStartDate3 = 17,
        InvalidKohiHokenMstEndDate3 = 18,
        InvalidKohiConfirmDate4 = 19,
        InvalidKohiHokenMstStartDate4 = 20,
        InvalidKohiHokenMstEndDate4 = 21,
        InvalidPatientInfBirthday = 22,
        InvalidPatternHokenKbn = 23,
        InvalidConfirmDateAgeCheck = 24,
        InvalidConfirmDateHoken = 25,
        InvalidHokenMstDate = 26,
        InvalidConfirmDateKohi1 = 27,
        InvalidMasterDateKohi1 = 28,
        InvalidConfirmDateKohi2 = 29,
        InvalidMasterDateKohi2 = 30,
        InvalidConfirmDateKohi3 = 31,
        InvalidMasterDateKohi3 = 32,
        InvalidConfirmDateKohi4 = 33,
        InvalidMasterDateKohi4 = 34,
        InvalidPatternIsExpirated = 35,
        InvalidHasElderHoken = 36,
    }
}
