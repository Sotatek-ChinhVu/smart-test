namespace UseCase.PatientInfor.Save
{
    public enum SavePatientInforValidationCode
    {
        InvalidHpId,
        InvalidPtId,
        InvalidName,
        InvalidKanaName,
        InvalidBirthday,
        InvalidIsDead,
        InvalidDeathDate,
        InvalidHomePost,
        InvalidHomeAddress1,
        InvalidHomeAddress2,
        InvalidTel1,
        InvalidTel2,
        InvalidMail,
        InvalidSetanusi,
        InvalidZokugara,
        InvalidJob,
        InvalidRenrakuPost,
        InvalidRenrakuAddress1,
        InvalidRenrakuAddress2,
        InvalidRenrakuTel,
        InvalidRenrakuMemo,
        InvalidOfficeName,
        InvalidOfficePost,
        InvalidOfficeAddress1,
        InvalidOfficeAddress2,
        InvalidOfficeTel,
        InvalidOfficeMemo,
        InvalidHokenPatternWhenUpdate,
        WarningRegisteredInsuranceCombination,
        WarningAgeCheck,
        WarningInsuranceElderlyLaterNotYetCovered,
        WarningLaterInsuranceRegisteredPatientsElderInsurance,
        ConfirmHokenPatternSelectedIsInfMainHokenPid,
        WarningHaveanExpiredHokenOnMain,
        WarningInsuranceSameInsuranceNumber,
        WarningMultipleHokenSignedUpSameTime,
        WarningFundsWithSamePayerCode,
        InvalidIncorrectinsuranceNumber,
        InvalidIncorrectLaborinsuranceNumber,
        InvalidIncorrectLaborinsuranceNumberMoreThan14,
        #region insurance
        ÍnuranceInvalidHokenId,
        ÍnuranceInvalidKohi1Id,
        ÍnuranceInvalidKohi2Id,
        ÍnuranceInvalidKohi3Id,
        ÍnuranceInvalidKohi4Id,
        #endregion Insurance
        #region PtKyusei
        PtKyuseiInvalidSeqNo,
        PtKyuseiInvalidKanaName,
        PtKyuseiInvalidName,
        PtKyuseiInvalidEndDate,
        #endregion PtKyusei
        #region PtSanteis
        PtSanteiInvalidSeqNo,
        PtSanteiInvalidEdaNo,
        PtSanteiInvalidKbnVal,
        PtSanteiInvalidStartDate,
        PtSanteiInvalidEndDate,
        #endregion PtSanteis
        #region PtGrps
        PtGrpInvalidGroupId,
        PtGrpInvalidGroupCode,
        #endregion PtGrps
    }
}
