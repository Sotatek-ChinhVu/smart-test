﻿namespace Domain.Models.PatientInfor
{
    public class ReactSavePatientInfo
    {
        public bool ConfirmHaveanExpiredHokenOnMain { get; set; }

        public bool ConfirmRegisteredInsuranceCombination { get; set; }

        public bool ConfirmAgeCheck { get; set; }

        public bool ConfirmInsuranceElderlyLaterNotYetCovered { get; set; }

        public bool ConfirmLaterInsuranceRegisteredPatientsElderInsurance { get; set; }

        public bool ConfirmInsuranceSameInsuranceNumber { get; set; }

        public bool ConfirmMultipleHokenSignedUpSameTime { get; set; }

        public bool ConfirmFundsWithSamePayerCode { get; set; }

        public bool ConfirmInvalidJiscodeCheck { get; set; }

        public bool ConfirmHokenPatternSelectedIsInfMainHokenPid { get; set; }

        public bool ConfirmSamePatientInf { get; set; }

        public bool ConfirmCloneByomei { get; set; }
    }
}
