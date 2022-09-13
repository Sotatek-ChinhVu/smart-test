using Domain.Models.InsuranceInfor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ReceptionInsurance
{
    public interface IReceptionInsuranceRepository
    {
        public IEnumerable<ReceptionInsuranceModel> GetReceptionInsurance(int hpId, long ptId, int sinDate, bool isShowExpiredReception);

        string CheckPatternExpirated(int hpId, long ptId, int sinDate, int patternHokenPid, bool patternIsExpirated, bool hokenInfIsJihi, bool hokenInfIsNoHoken, int patternConfirmDate, int hokenInfStartDate, int hokenInfEndDate, bool isHaveHokenMst, int hokenMstStartDate, int hokenMstEndDate, string hokenMstDisplayTextMaster, bool isEmptyKohi1, bool isKohiHaveHokenMst1, int kohiConfirmDate1, string kohiHokenMstDisplayTextMaster1, int kohiHokenMstStartDate1, int kohiHokenMstEndDate1, bool isEmptyKohi2, bool isKohiHaveHokenMst2, int kohiConfirmDate2, string kohiHokenMstDisplayTextMaster2, int kohiHokenMstStartDate2, int kohiHokenMstEndDate2, bool isEmptyKohi3, bool isKohiHaveHokenMst3, int kohiConfirmDate3, string kohiHokenMstDisplayTextMaster3, int kohiHokenMstStartDate3, int kohiHokenMstEndDate3, bool isEmptyKohi4, bool isKohiHaveHokenMst4, int kohiConfirmDate4, string kohiHokenMstDisplayTextMaster4, int kohiHokenMstStartDate4, int kohiHokenMstEndDate4, int patientInfBirthday, int patternHokenKbn);
    }
}
