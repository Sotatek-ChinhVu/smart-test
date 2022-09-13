using Domain.Models.ReceptionInsurance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Insurance.ValidPatternExpirated;

namespace Interactor.Insurance
{
    public class ValidPatternExpiratedInteractor : IValidPatternExpiratedInputPort
    {
        private readonly IReceptionInsuranceRepository _insuranceResponsitory;
        public ValidPatternExpiratedInteractor(IReceptionInsuranceRepository insuranceResponsitory)
        {
            _insuranceResponsitory = insuranceResponsitory;
        }

        public ValidPatternExpiratedOutputData Handle(ValidPatternExpiratedInputData inputData)
        {
            if (inputData.HpId < 0)
            {
                return new ValidPatternExpiratedOutputData(false, string.Empty, ValidPatternExpiratedStatus.InvalidHpId);
            }

            if (inputData.PtId < 0)
            {
                return new ValidPatternExpiratedOutputData(false, string.Empty, ValidPatternExpiratedStatus.InvalidPtId);
            }

            if (inputData.SinDate < 0)
            {
                return new ValidPatternExpiratedOutputData(false, string.Empty, ValidPatternExpiratedStatus.InvalidSinDate);
            }
            
            if (inputData.PatternHokenPid < 0)
            {
                return new ValidPatternExpiratedOutputData(false, string.Empty, ValidPatternExpiratedStatus.InvalidPatternHokenPid);
            }
            
            if (inputData.PatternConfirmDate < 0)
            {
                return new ValidPatternExpiratedOutputData(false, string.Empty, ValidPatternExpiratedStatus.InvalidPatternConfirmDate);
            }
            
            if (inputData.HokenInfStartDate < 0)
            {
                return new ValidPatternExpiratedOutputData(false, string.Empty, ValidPatternExpiratedStatus.InvalidHokenInfStartDate);
            }

            if (inputData.HokenInfEndDate < 0)
            {
                return new ValidPatternExpiratedOutputData(false, string.Empty, ValidPatternExpiratedStatus.InvalidHokenInfEndDate);
            }

            if (inputData.HokenMstStartDate < 0)
            {
                return new ValidPatternExpiratedOutputData(false, string.Empty, ValidPatternExpiratedStatus.InvalidHokenMstStartDate);
            }

            if (inputData.HokenMstEndDate < 0)
            {
                return new ValidPatternExpiratedOutputData(false, string.Empty, ValidPatternExpiratedStatus.InvalidHokenMstEndDate);
            }

            if (inputData.KohiConfirmDate1 < 0)
            {
                return new ValidPatternExpiratedOutputData(false, string.Empty, ValidPatternExpiratedStatus.InvalidKohiConfirmDate1);
            }

            if (inputData.KohiHokenMstStartDate1 < 0)
            {
                return new ValidPatternExpiratedOutputData(false, string.Empty, ValidPatternExpiratedStatus.InvalidKohiHokenMstStartDate1);
            }

            if (inputData.KohiHokenMstEndDate1 < 0)
            {
                return new ValidPatternExpiratedOutputData(false, string.Empty, ValidPatternExpiratedStatus.InvalidKohiHokenMstEndDate1);
            }

            if (inputData.KohiConfirmDate2 < 0)
            {
                return new ValidPatternExpiratedOutputData(false, string.Empty, ValidPatternExpiratedStatus.InvalidKohiConfirmDate2);
            }

            if (inputData.KohiHokenMstStartDate2 < 0)
            {
                return new ValidPatternExpiratedOutputData(false, string.Empty, ValidPatternExpiratedStatus.InvalidKohiHokenMstStartDate2);
            }

            if (inputData.KohiHokenMstEndDate2 < 0)
            {
                return new ValidPatternExpiratedOutputData(false, string.Empty, ValidPatternExpiratedStatus.InvalidKohiHokenMstEndDate2);
            }

            if (inputData.KohiConfirmDate3 < 0)
            {
                return new ValidPatternExpiratedOutputData(false, string.Empty, ValidPatternExpiratedStatus.InvalidKohiConfirmDate3);
            }

            if (inputData.KohiHokenMstStartDate3 < 0)
            {
                return new ValidPatternExpiratedOutputData(false, string.Empty, ValidPatternExpiratedStatus.InvalidKohiHokenMstStartDate3);
            }

            if (inputData.KohiHokenMstEndDate3 < 0)
            {
                return new ValidPatternExpiratedOutputData(false, string.Empty, ValidPatternExpiratedStatus.InvalidKohiHokenMstEndDate3);
            }
            

            if (inputData.KohiConfirmDate4 < 0)
            {
                return new ValidPatternExpiratedOutputData(false, string.Empty, ValidPatternExpiratedStatus.InvalidKohiConfirmDate4);
            }

            if (inputData.KohiHokenMstStartDate4 < 0)
            {
                return new ValidPatternExpiratedOutputData(false, string.Empty, ValidPatternExpiratedStatus.InvalidKohiHokenMstStartDate4);
            }

            if (inputData.KohiHokenMstEndDate4 < 0)
            {
                return new ValidPatternExpiratedOutputData(false, string.Empty, ValidPatternExpiratedStatus.InvalidKohiHokenMstEndDate4);
            }

            if (inputData.PatientInfBirthday < 0)
            {
                return new ValidPatternExpiratedOutputData(false, string.Empty, ValidPatternExpiratedStatus.InvalidPatientInfBirthday);
            }

            if (inputData.PatternHokenKbn < 0)
            {
                return new ValidPatternExpiratedOutputData(false, string.Empty, ValidPatternExpiratedStatus.InvalidPatternHokenKbn);
            }

            var dataMessage = _insuranceResponsitory.CheckPatternExpirated(inputData.HpId, 
                                                                           inputData.PtId,
                                                                           inputData.SinDate,
                                                                           inputData.PatternHokenPid,
                                                                           inputData.PatternIsExpirated,
                                                                           inputData.HokenInfIsJihi,
                                                                           inputData.HokenInfIsNoHoken,
                                                                           inputData.PatternConfirmDate,
                                                                           inputData.HokenInfStartDate,
                                                                           inputData.HokenInfEndDate,
                                                                           inputData.IsHaveHokenMst,
                                                                           inputData.HokenMstStartDate,
                                                                           inputData.HokenMstEndDate,
                                                                           inputData.HokenMstDisplayTextMaster,
                                                                           inputData.IsEmptyKohi1,
                                                                           inputData.IsKohiHaveHokenMst1,
                                                                           inputData.KohiConfirmDate1,
                                                                           inputData.KohiHokenMstDisplayTextMaster1,
                                                                           inputData.KohiHokenMstStartDate1,
                                                                           inputData.KohiHokenMstEndDate1,
                                                                           inputData.IsEmptyKohi2,
                                                                           inputData.IsKohiHaveHokenMst2,
                                                                           inputData.KohiConfirmDate2,
                                                                           inputData.KohiHokenMstDisplayTextMaster2,
                                                                           inputData.KohiHokenMstStartDate2,
                                                                           inputData.KohiHokenMstEndDate2,
                                                                           inputData.IsEmptyKohi3,
                                                                           inputData.IsKohiHaveHokenMst3,
                                                                           inputData.KohiConfirmDate3,
                                                                           inputData.KohiHokenMstDisplayTextMaster3,
                                                                           inputData.KohiHokenMstStartDate3,
                                                                           inputData.KohiHokenMstEndDate3,
                                                                           inputData.IsEmptyKohi4,
                                                                           inputData.IsKohiHaveHokenMst4,
                                                                           inputData.KohiConfirmDate4,
                                                                           inputData.KohiHokenMstDisplayTextMaster4,
                                                                           inputData.KohiHokenMstStartDate4,
                                                                           inputData.KohiHokenMstEndDate4,
                                                                           inputData.PatientInfBirthday,
                                                                           inputData.PatternHokenKbn
                                                                           );
            return new ValidPatternExpiratedOutputData(String.IsNullOrEmpty(dataMessage), dataMessage, ValidPatternExpiratedStatus.ValidPatternExpiratedSuccess);
        }
    }
}
