namespace UseCase.Diseases.Upsert
{
    public enum UpsertPtDiseaseListStatus
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
    }
}
