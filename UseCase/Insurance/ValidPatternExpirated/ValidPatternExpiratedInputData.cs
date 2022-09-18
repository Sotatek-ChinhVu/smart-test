using Domain.Models.InsuranceInfor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.Insurance.ValidPatternExpirated
{
    public class ValidPatternExpiratedInputData : IInputData<ValidPatternExpiratedOutputData>
    {
        public ValidPatternExpiratedInputData(int hpId, long ptId, int sinDate, int patternHokenPid, bool patternIsExpirated, bool hokenInfIsJihi, bool hokenInfIsNoHoken, int patternConfirmDate, int hokenInfStartDate, int hokenInfEndDate, bool isHaveHokenMst, int hokenMstStartDate, int hokenMstEndDate, string hokenMstDisplayTextMaster, bool isEmptyKohi1, bool isKohiHaveHokenMst1, int kohiConfirmDate1, string kohiHokenMstDisplayTextMaster1, int kohiHokenMstStartDate1, int kohiHokenMstEndDate1, bool isEmptyKohi2, bool isKohiHaveHokenMst2, int kohiConfirmDate2, string kohiHokenMstDisplayTextMaster2, int kohiHokenMstStartDate2, int kohiHokenMstEndDate2, bool isEmptyKohi3, bool isKohiHaveHokenMst3, int kohiConfirmDate3, string kohiHokenMstDisplayTextMaster3, int kohiHokenMstStartDate3, int kohiHokenMstEndDate3, bool isEmptyKohi4, bool isKohiHaveHokenMst4, int kohiConfirmDate4, string kohiHokenMstDisplayTextMaster4, int kohiHokenMstStartDate4, int kohiHokenMstEndDate4, int patientInfBirthday, int patternHokenKbn)
        {
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
            PatternHokenPid = patternHokenPid;
            PatternIsExpirated = patternIsExpirated;
            HokenInfIsJihi = hokenInfIsJihi;
            HokenInfIsNoHoken = hokenInfIsNoHoken;
            PatternConfirmDate = patternConfirmDate;
            HokenInfStartDate = hokenInfStartDate;
            HokenInfEndDate = hokenInfEndDate;
            IsHaveHokenMst = isHaveHokenMst;
            HokenMstStartDate = hokenMstStartDate;
            HokenMstEndDate = hokenMstEndDate;
            HokenMstDisplayTextMaster = hokenMstDisplayTextMaster;
            IsEmptyKohi1 = isEmptyKohi1;
            IsKohiHaveHokenMst1 = isKohiHaveHokenMst1;
            KohiConfirmDate1 = kohiConfirmDate1;
            KohiHokenMstDisplayTextMaster1 = kohiHokenMstDisplayTextMaster1;
            KohiHokenMstStartDate1 = kohiHokenMstStartDate1;
            KohiHokenMstEndDate1 = kohiHokenMstEndDate1;
            IsEmptyKohi2 = isEmptyKohi2;
            IsKohiHaveHokenMst2 = isKohiHaveHokenMst2;
            KohiConfirmDate2 = kohiConfirmDate2;
            KohiHokenMstDisplayTextMaster2 = kohiHokenMstDisplayTextMaster2;
            KohiHokenMstStartDate2 = kohiHokenMstStartDate2;
            KohiHokenMstEndDate2 = kohiHokenMstEndDate2;
            IsEmptyKohi3 = isEmptyKohi3;
            IsKohiHaveHokenMst3 = isKohiHaveHokenMst3;
            KohiConfirmDate3 = kohiConfirmDate3;
            KohiHokenMstDisplayTextMaster3 = kohiHokenMstDisplayTextMaster3;
            KohiHokenMstStartDate3 = kohiHokenMstStartDate3;
            KohiHokenMstEndDate3 = kohiHokenMstEndDate3;
            IsEmptyKohi4 = isEmptyKohi4;
            IsKohiHaveHokenMst4 = isKohiHaveHokenMst4;
            KohiConfirmDate4 = kohiConfirmDate4;
            KohiHokenMstDisplayTextMaster4 = kohiHokenMstDisplayTextMaster4;
            KohiHokenMstStartDate4 = kohiHokenMstStartDate4;
            KohiHokenMstEndDate4 = kohiHokenMstEndDate4;
            PatientInfBirthday = patientInfBirthday;
            PatternHokenKbn = patternHokenKbn;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int SinDate { get; private set; }

        public int PatternHokenPid { get; private set; }

        public bool PatternIsExpirated { get; private set; }

        public bool HokenInfIsJihi { get; private set; }

        public bool HokenInfIsNoHoken { get; private set; }

        public int PatternConfirmDate { get; private set; }

        public int HokenInfStartDate { get; private set; }

        public int HokenInfEndDate { get; private set; }

        public bool IsHaveHokenMst { get; private set; }

        public int HokenMstStartDate { get; private set; }

        public int HokenMstEndDate { get; private set; }

        public string HokenMstDisplayTextMaster { get; private set; }

        public bool IsEmptyKohi1 { get; private set; }

        public bool IsKohiHaveHokenMst1 { get; private set; }

        public int KohiConfirmDate1 { get; private set; }

        public string KohiHokenMstDisplayTextMaster1 { get; private set; }

        public int KohiHokenMstStartDate1 { get; private set; }

        public int KohiHokenMstEndDate1 { get; private set; }

        public bool IsEmptyKohi2 { get; private set; }

        public bool IsKohiHaveHokenMst2 { get; private set; }

        public int KohiConfirmDate2 { get; private set; }

        public string KohiHokenMstDisplayTextMaster2 { get; private set; }

        public int KohiHokenMstStartDate2 { get; private set; }

        public int KohiHokenMstEndDate2 { get; private set; }

        public bool IsEmptyKohi3 { get; private set; }

        public bool IsKohiHaveHokenMst3 { get; private set; }

        public int KohiConfirmDate3 { get; private set; }

        public string KohiHokenMstDisplayTextMaster3 { get; private set; }

        public int KohiHokenMstStartDate3 { get; private set; }

        public int KohiHokenMstEndDate3 { get; private set; }

        public bool IsEmptyKohi4 { get; private set; }

        public bool IsKohiHaveHokenMst4 { get; private set; }

        public int KohiConfirmDate4 { get; private set; }

        public string KohiHokenMstDisplayTextMaster4 { get; private set; }

        public int KohiHokenMstStartDate4 { get; private set; }

        public int KohiHokenMstEndDate4 { get; private set; }

        public int PatientInfBirthday { get; private set; }

        public int PatternHokenKbn { get; private set; }
    }
}
