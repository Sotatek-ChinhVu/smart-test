using EmrCloudApi.Tenant.Constants;
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
                Data = new SearchInputItemResponse(outputData.ListInputModel, outputData.ListInputModel.Count),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case SearchInputItemStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case SearchInputItemStatus.InValidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case SearchInputItemStatus.InValidKeyword:
                    Result.Message = ResponseMessage.InvalidKeyword;
                    break;
                case SearchInputItemStatus.InvalidSindate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case SearchInputItemStatus.InvalidStartIndex:
                    Result.Message = ResponseMessage.InvalidStartIndex;
                    break;
                case SearchInputItemStatus.InvalidPageCount:
                    Result.Message = ResponseMessage.InvalidPageCount;
                    break;
                case SearchInputItemStatus.InvalidKouiKbn:
                    Result.Message = ResponseMessage.InvalidKouiKbn;
                    break;
            }
        }
    }
}
