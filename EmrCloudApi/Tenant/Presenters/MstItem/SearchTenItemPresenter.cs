using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.MstItem;
using UseCase.MstItem.SearchTenItem;

namespace EmrCloudApi.Tenant.Presenters.MstItem
{
    public class SearchTenItemPresenter : ISearchTenItemOutputPort
    {
        public Response<SearchTenItemResponse> Result { get; private set; } = default!;

        public void Complete(SearchTenItemOutputData outputData)
        {
            Result = new Response<SearchTenItemResponse>
            {
                Data = new SearchTenItemResponse(outputData.ListInputModel,outputData.TotalCount),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case SearchTenItemStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case SearchTenItemStatus.InValidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case SearchTenItemStatus.InValidKeyword:
                    Result.Message = ResponseMessage.InvalidKeyword;
                    break;
                case SearchTenItemStatus.InvalidSindate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case SearchTenItemStatus.InvalidPageIndex:
                    Result.Message = ResponseMessage.InvalidPageIndex;
                    break;
                case SearchTenItemStatus.InvalidPageCount:
                    Result.Message = ResponseMessage.InvalidPageCount;
                    break;
                case SearchTenItemStatus.InvalidKouiKbn:
                    Result.Message = ResponseMessage.InvalidKouiKbn;
                    break;
            }
        }
    }
}
