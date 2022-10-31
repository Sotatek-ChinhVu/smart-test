using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.MstItem;
using UseCase.MstItem.SearchPostCode;

namespace EmrCloudApi.Tenant.Presenters.MstItem
{
    public class SearchPostCodePresenter : ISearchPostCodeOutputPort
    {
        public Response<SearchPostCodeRespone> Result { get; private set; } = new Response<SearchPostCodeRespone>();

        public void Complete(SearchPostCodeOutputData outputData)
        {
            Result.Data = new SearchPostCodeRespone(outputData.TotalCount, outputData.PostCodeMstModels);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }

        private string GetMessage(SearchPostCodeStatus status) => status switch
        {
            SearchPostCodeStatus.Success => ResponseMessage.Success,
            SearchPostCodeStatus.Failed => ResponseMessage.Failed,
            SearchPostCodeStatus.NoData => ResponseMessage.NoData,
            SearchPostCodeStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            SearchPostCodeStatus.InvalidPostCode => ResponseMessage.InvalidPostCode,
            SearchPostCodeStatus.InvalidPageIndex => ResponseMessage.InvalidPageIndex,
            SearchPostCodeStatus.InvalidPageSize => ResponseMessage.InvalidPageCount,
            _ => string.Empty
        };
    }
}
