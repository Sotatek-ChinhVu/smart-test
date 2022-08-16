using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.InputItem;
using UseCase.InputItem.Search;

namespace EmrCloudApi.Tenant.Presenters.InputItem
{
    public class SearchInputItemPresenter : ISearchInputItemOutputPort
    {
        public Response<SearchInputItemResponse> Result { get; private set; } = default!;

        public void Complete(SearchInputItemOutputData outputData)
        {
            Result = new Response<SearchInputItemResponse>
            {
                Data = new SearchInputItemResponse(outputData.ListInputModel),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case SearchInputItemStatus.Successed:
                    break;
                case SearchInputItemStatus.InValidHpId:
                    break;
                case SearchInputItemStatus.InValidKeyword:
                    break;
                case SearchInputItemStatus.InvalidSindate:
                    break;
                case SearchInputItemStatus.InvalidStartIndex:
                    break;
                case SearchInputItemStatus.InvalidPageCount:
                    break;
                case SearchInputItemStatus.InvalidKouiKbn:
                    break;
                default:
                    break;
            }
        }
    }
}
