using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MstItem;
using UseCase.MstItem.SearchTenMstItem;

namespace EmrCloudApi.Presenters.MstItem
{
    public class SearchTenMstItemPresenter
    {
        public Response<SearchTenMstItemResponse> Result { get; private set; } = default!;

        public void Complete(SearchTenMstItemOutputData outputData)
        {
            Result = new Response<SearchTenMstItemResponse>
            {
                Data = new SearchTenMstItemResponse(outputData.TenMsts.Select(x => new TenItemDto(x)).ToList(), outputData.TotalCount),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case SearchTenMstItemStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case SearchTenMstItemStatus.InValidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case SearchTenMstItemStatus.InValidKeyword:
                    Result.Message = ResponseMessage.InvalidKeyword;
                    break;
                case SearchTenMstItemStatus.InvalidSindate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case SearchTenMstItemStatus.InvalidPageIndex:
                    Result.Message = ResponseMessage.InvalidPageIndex;
                    break;
                case SearchTenMstItemStatus.InvalidPageCount:
                    Result.Message = ResponseMessage.InvalidPageCount;
                    break;
                case SearchTenMstItemStatus.InvalidKouiKbn:
                    Result.Message = ResponseMessage.InvalidKouiKbn;
                    break;
            }
        }
    }
}
