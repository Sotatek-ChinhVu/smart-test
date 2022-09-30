using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.PatientInfor;
using UseCase.PatientInfor.SearchEmptyId;

namespace EmrCloudApi.Tenant.Presenters.PatientInfor
{
    public class SearchEmptyIdPresenter : ISearchEmptyOutputPort
    {
        public Response<SearchEmptyIdResponse> Result { get; private set; } = new Response<SearchEmptyIdResponse>();

        public void Complete(SearchEmptyIdOutputData outputData)
        {
            Result.Data = new SearchEmptyIdResponse(outputData.PatientInforModels);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }

        private string GetMessage(SearchEmptyIdStatus status) => status switch
        {
            SearchEmptyIdStatus.Success => ResponseMessage.Success,
            SearchEmptyIdStatus.Failed => ResponseMessage.Failed,
            SearchEmptyIdStatus.NoData => ResponseMessage.NoData,
            _ => string.Empty
        };
    }
}
