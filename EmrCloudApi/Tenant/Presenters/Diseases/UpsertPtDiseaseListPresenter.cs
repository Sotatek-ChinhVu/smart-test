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
            UpsertPtDiseaseListStatus.Success => ResponseMessage.UpsertPtDiseaseSuccess,
            UpsertPtDiseaseListStatus.PtDiseaseListUpdateNoSuccess => ResponseMessage.UpsertPtDiseaseFail,
            UpsertPtDiseaseListStatus.PtDiseaseListInputNoData => ResponseMessage.UpsertPtDiseaseInputNoData,
            _ => string.Empty
        };
    }
}
