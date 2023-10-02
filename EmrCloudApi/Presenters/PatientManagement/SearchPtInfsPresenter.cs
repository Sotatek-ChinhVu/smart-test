using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.PatientManagement;
using UseCase.PatientManagement.SearchPtInfs;

namespace EmrCloudApi.Presenters.PatientManagement
{
    public class SearchPtInfsPresenter : ISearchPtInfsOutputPort
    {
        public Response<SearchPtInfsResponse> Result { get; private set; } = new Response<SearchPtInfsResponse>();

        public void Complete(SearchPtInfsOutputData outputData)
        {
            Result.Data = new SearchPtInfsResponse(outputData.TotalCount, outputData.PtInfs);
            Result.Status = (int)outputData.Status;
            Result.Message = GetMessage(outputData.Status);
        }

        private string GetMessage(SearchPtInfsStatus status) => status switch
        {
            SearchPtInfsStatus.Successed => ResponseMessage.Success,
            SearchPtInfsStatus.InvalidPageCount => ResponseMessage.InvalidPageCount,
            SearchPtInfsStatus.InvalidPageIndex => ResponseMessage.InvalidPageIndex,
            SearchPtInfsStatus.NoData => ResponseMessage.NoData,
            _ => string.Empty
        };
    }
}