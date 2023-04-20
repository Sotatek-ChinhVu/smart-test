namespace UseCase.Diseases.Validation
{
    public enum ValidationPtDiseaseListStatus
    {
        Success = 1,
        PtDiseaseListInputNoData = 2,
        PtDiseaseListUpdateNoSuccess = 3,
        PtDiseaseListPtIdNoExist,
        PtDiseaseListHokenPIdNoExist,
        PtDiseaseListInvalidTenkiKbn,
        PtDiseaseListInvalidSikkanKbn,
        PtDiseaseListInvalidNanByoCd,
        PtDiseaseListInvalidFreeWord,
        PtDiseaseListInvalidTenkiDateContinue,
        PtDiseaseListInvalidTenkiDateCommon,
        PtDiseaseListInvalidTekiDateAndStartDate,
        PtDiseaseListInvalidByomei,
        PtInvalidId,
        PtInvalidHpId,
        PtInvalidPtId,
        PtInvalidSortNo,
        PtInvalidByomeiCd,
        PtInvalidStartDate,
        PtInvalidTenkiDate,
        PtInvalidSyubyoKbn,
        PtInvalidHosokuCmt,
        PtInvalidHokenPid,
        PtInvalidIsNodspRece,
        PtInvalidIsNodspKarte,
        PtInvalidSeqNo,
        PtInvalidIsImportant,
        PtInvalidIsDeleted,
        Valid,
        Failed
    }
}
