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
            Result.Data = new UpsertPtDiseaseListResponse(output.Ids);
            Result.Message = GetMessage(output.Status);
            Result.Status = (int)output.Status;
        }

        private string GetMessage(UpsertPtDiseaseListStatus status) => status switch
        {
            UpsertPtDiseaseListStatus.Success => ResponseMessage.PtDiseaseUpsertSuccess,
            UpsertPtDiseaseListStatus.PtDiseaseListUpdateNoSuccess => ResponseMessage.PtDiseaseUpsertFail,
            UpsertPtDiseaseListStatus.PtDiseaseListInputNoData => ResponseMessage.PtDiseaseUpsertInputNoData,
            UpsertPtDiseaseListStatus.PtDiseaseListInvalidTenkiKbn => ResponseMessage.MCommonError,
            UpsertPtDiseaseListStatus.PtDiseaseListInvalidSikkanKbn => ResponseMessage.MCommonError,
            UpsertPtDiseaseListStatus.PtDiseaseListInvalidNanByoCd => ResponseMessage.MCommonError,
            UpsertPtDiseaseListStatus.PtDiseaseListPtIdNoExist => ResponseMessage.MCommonError,
            UpsertPtDiseaseListStatus.PtDiseaseListHokenPIdNoExist => ResponseMessage.MCommonError,
            UpsertPtDiseaseListStatus.PtDiseaseListInvalidFreeWord => ResponseMessage.MEnt00040_1,
            UpsertPtDiseaseListStatus.PtDiseaseListInvalidTenkiDateContinue => string.Format(ResponseMessage.MInp00010, ResponseMessage.MTenkiContinue),
            UpsertPtDiseaseListStatus.PtDiseaseListInvalidTenkiDateCommon => string.Format(ResponseMessage.MInp00010, ResponseMessage.MTenkiDate),
            UpsertPtDiseaseListStatus.PtDiseaseListInvalidTekiDateAndStartDate => string.Format(ResponseMessage.MInp00110, ResponseMessage.MTenkiDate, ResponseMessage.MTenkiStartDate),
            UpsertPtDiseaseListStatus.PtDiseaseListInvalidByomei => string.Format(ResponseMessage.MInp00160_1, ResponseMessage.MDisease),
            UpsertPtDiseaseListStatus.PtInvalidId =>
              ResponseMessage.MCommonError,
            UpsertPtDiseaseListStatus.PtInvalidHpId =>
              ResponseMessage.MCommonError,
            UpsertPtDiseaseListStatus.PtInvalidPtId =>
              ResponseMessage.MCommonError,
            UpsertPtDiseaseListStatus.PtInvalidSortNo =>
              ResponseMessage.MCommonError,
            UpsertPtDiseaseListStatus.PtInvalidByomeiCd =>
              ResponseMessage.MCommonError,
            UpsertPtDiseaseListStatus.PtInvalidStartDate =>
              ResponseMessage.MTenkiStartDate_2,
            UpsertPtDiseaseListStatus.PtInvalidTenkiDate =>
              ResponseMessage.MCommonError,
            UpsertPtDiseaseListStatus.PtInvalidSyubyoKbn =>
              ResponseMessage.MCommonError,
            UpsertPtDiseaseListStatus.PtInvalidHosokuCmt =>
              ResponseMessage.MCommonError,
            UpsertPtDiseaseListStatus.PtInvalidHokenPid =>
              ResponseMessage.MCommonError,
            UpsertPtDiseaseListStatus.PtInvalidIsNodspRece =>
              ResponseMessage.MCommonError,
            UpsertPtDiseaseListStatus.PtInvalidIsNodspKarte =>
              ResponseMessage.MCommonError,
            UpsertPtDiseaseListStatus.PtInvalidSeqNo =>
              ResponseMessage.MCommonError,
            UpsertPtDiseaseListStatus.PtInvalidIsImportant =>
              ResponseMessage.MCommonError,
            UpsertPtDiseaseListStatus.PtInvalidIsDeleted =>
              ResponseMessage.MCommonError,
            _ => string.Empty
        };
    }
}
