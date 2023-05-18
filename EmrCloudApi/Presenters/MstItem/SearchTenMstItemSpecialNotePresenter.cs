using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MstItem;
using UseCase.MstItem.SearchTenMstItemSpecialNote;

namespace EmrCloudApi.Presenters.MstItem
{
    public class SearchTenMstItemSpecialNotePresenter : ISearchTenMstItemSpecialNoteOutputPort
    {
        public Response<SearchTenMstSpecialNoteResponse> Result { get; private set; } = default!;

        public void Complete(SearchTenMstItemSpecialNoteOutputData outputData)
        {
            Result = new Response<SearchTenMstSpecialNoteResponse>
            {
                Data = new SearchTenMstSpecialNoteResponse(outputData.ListInputModel.Select(x => new TenItemDto(x)).ToList(), outputData.TotalCount),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case SearchTenMstItemSpecialNoteStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case SearchTenMstItemSpecialNoteStatus.InValidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case SearchTenMstItemSpecialNoteStatus.InValidKeyword:
                    Result.Message = ResponseMessage.InvalidKeyword;
                    break;
                case SearchTenMstItemSpecialNoteStatus.InvalidSindate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case SearchTenMstItemSpecialNoteStatus.InvalidPageIndex:
                    Result.Message = ResponseMessage.InvalidPageIndex;
                    break;
                case SearchTenMstItemSpecialNoteStatus.InvalidPageCount:
                    Result.Message = ResponseMessage.InvalidPageCount;
                    break;
                case SearchTenMstItemSpecialNoteStatus.InvalidKouiKbn:
                    Result.Message = ResponseMessage.InvalidKouiKbn;
                    break;
            }
        }
    }
}
