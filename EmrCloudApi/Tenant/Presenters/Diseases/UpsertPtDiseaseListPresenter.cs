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
            _ => string.Empty
        };
    }
}
