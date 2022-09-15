using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.MstItem;
using UseCase.MstItem.SearchSupplement;

namespace EmrCloudApi.Tenant.Presenters.MstItem
{
    public class SearchSupplementPresenter : ISearchSupplementPresenterOutputPort
    {
        public Response<SearchSupplementResponse> Result { get; private set; } = default!;

        public void Complete(SearchSupplementOutputData outputData)
        {
            Result = new Response<SearchSupplementResponse>
            {
                Data = new SearchSupplementResponse(outputData.SearchSupplementResponse,outputData.Total),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case SearchSupplementStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case SearchSupplementStatus.Fail:
                    Result.Message = ResponseMessage.Failed;
                    break;
                case SearchSupplementStatus.NoData:
                    Result.Message = ResponseMessage.NoData;
                    break;
            }
        }
    }
}
