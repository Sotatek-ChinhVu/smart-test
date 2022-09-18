using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.Diseases;
using UseCase.Diseases.Upsert;

namespace EmrCloudApi.Tenant.Presenters.Diseases
{
    public class UpsertPtDiseaseListPresenter : IUpsertPtDiseaseListOutputPort
    {
        public Response<UpsertPtDiseaseListResponse> Result { get; private set; } = new Response<UpsertPtDiseaseListResponse>();

        public void Complete(UpsertPtDiseaseListOutputData output)
        {
            Result.Data = new UpsertPtDiseaseListResponse(output.Status == UpsertPtDiseaseListStatus.Success);
            Result.Message = GetMessage(output.Status);
            Result.Status = (int)output.Status;
        }

        private string GetMessage(UpsertPtDiseaseListStatus status) => status switch
        {
            UpsertPtDiseaseListStatus.Success => ResponseMessage.UpsertPtDiseaseListSuccess,
            UpsertPtDiseaseListStatus.PtDiseaseListUpdateNoSuccess => ResponseMessage.UpsertPtDiseaseListFail,
            UpsertPtDiseaseListStatus.PtDiseaseListInputNoData => ResponseMessage.UpsertPtDiseaseListInputNoData,
            UpsertPtDiseaseListStatus.PtDiseaseListInvalidTenkiKbn => ResponseMessage.UpsertPtDiseaseListInvalidTenkiKbn,
            UpsertPtDiseaseListStatus.PtDiseaseListInvalidSikkanKbn => ResponseMessage.UpsertPtDiseaseListInvalidSikkanKbn,
            UpsertPtDiseaseListStatus.PtDiseaseListInvalidNanByoCd => ResponseMessage.UpsertPtDiseaseListInvalidNanByoCd,
            UpsertPtDiseaseListStatus.PtDiseaseListPtIdNoExist => ResponseMessage.UpsertPtDiseaseListPtIdNoExist,
            UpsertPtDiseaseListStatus.PtDiseaseListHokenPIdNoExist => ResponseMessage.UpsertPtDiseaseListHokenPIdNoExist,
            UpsertPtDiseaseListStatus.PtDiseaseListInvalidFreeWord => ResponseMessage.UpsertPtDiseaseListInvalidFreeWord,
            UpsertPtDiseaseListStatus.PtDiseaseListInvalidTenkiDateContinue => ResponseMessage.UpsertPtDiseaseListInvalidTenkiDateContinue,
            UpsertPtDiseaseListStatus.PtDiseaseListInvalidTekiDateAndStartDate => ResponseMessage.UpsertPtDiseaseListInvalidTenkiDateAndStartDate,
            UpsertPtDiseaseListStatus.PtDiseaseListInvalidByomei => ResponseMessage.UpsertPtDiseaseListInvalidByomei,
            UpsertPtDiseaseListStatus.PtInvalidId =>
              ResponseMessage.UpsertPtDiseaseListInvalidId,
            UpsertPtDiseaseListStatus.PtInvalidHpId =>
              ResponseMessage.UpsertPtDiseaseListInvalidHpId,
            UpsertPtDiseaseListStatus.PtInvalidPtId =>
              ResponseMessage.UpsertPtDiseaseListInvalidPtId,
            UpsertPtDiseaseListStatus.PtInvalidSortNo =>
              ResponseMessage.UpsertPtDiseaseListInvalidSortNo,
            UpsertPtDiseaseListStatus.PtInvalidByomeiCd =>
              ResponseMessage.UpsertPtDiseaseListInvalidByomeiCd,
            UpsertPtDiseaseListStatus.PtInvalidStartDate =>
              ResponseMessage.UpsertPtDiseaseListInvalidStartDate,
            UpsertPtDiseaseListStatus.PtInvalidTenkiDate =>
              ResponseMessage.UpsertPtDiseaseListInvalidTenkiDate,
            UpsertPtDiseaseListStatus.PtInvalidSyubyoKbn =>
              ResponseMessage.UpsertPtDiseaseListInvalidSyubyoKbn,
            UpsertPtDiseaseListStatus.PtInvalidHosokuCmt =>
              ResponseMessage.UpsertPtDiseaseListInvalidHosokuCmt,
            UpsertPtDiseaseListStatus.PtInvalidHokenPid =>
              ResponseMessage.UpsertPtDiseaseListInvalidHokenPid,
            UpsertPtDiseaseListStatus.PtInvalidIsNodspRece =>
              ResponseMessage.UpsertPtDiseaseListInvalidIsNodspRece,
            UpsertPtDiseaseListStatus.PtInvalidIsNodspKarte =>
              ResponseMessage.UpsertPtDiseaseListInvalidIsNodspKarte,
            UpsertPtDiseaseListStatus.PtInvalidSeqNo =>
              ResponseMessage.UpsertPtDiseaseListInvalidSeqNo,
            UpsertPtDiseaseListStatus.PtInvalidIsImportant =>
              ResponseMessage.UpsertPtDiseaseListInvalidIsImportant,
            UpsertPtDiseaseListStatus.PtInvalidIsDeleted =>
              ResponseMessage.UpsertPtDiseaseListInvalidIsDeleted,
            _ => string.Empty
        };
    }
}
