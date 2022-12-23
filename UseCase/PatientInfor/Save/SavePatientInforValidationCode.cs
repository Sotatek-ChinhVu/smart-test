namespace UseCase.PatientInfor.Save
{
    public enum SavePatientInforValidationCode
    {
        InvalidHpId,
        InvalidPtId,
        InvalidName,
        InvalidKanaName,
        Birthday,
        IsDead,
        HomePost,
        HomeAddress1,
        HomeAddress2,
        Tel1,
        Tel2,
        Mail,
        Setanusi,
        Zokugara,
        Job,
        RenrakuPost,
        RenrakuAddress1,
        RenrakuAddress2,
        RenrakuTel,
        RenrakuMemo,
        OfficeName,
        OfficePost,
        OfficeAddress1,
        OfficeAddress2,
        OfficeTel,
        OfficeMemo,
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
