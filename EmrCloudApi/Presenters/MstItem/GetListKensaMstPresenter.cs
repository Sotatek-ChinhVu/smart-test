using EmrCloudApi.Responses.MstItem;
using EmrCloudApi.Responses;
using UseCase.MstItem.GetListResultKensaMst;
using UseCase.MstItem.SearchPostCode;
using EmrCloudApi.Constants;
using UseCase.MstItem.SearchTenMstItem;

namespace EmrCloudApi.Presenters.MstItem
{
    public class GetListKensaMstPresenter : IGetListKensaMstOutputPort
    {
        public Response<GetListKensaMstResponse> Result { get; private set; } = new Response<GetListKensaMstResponse>();
        public void Complete(GetListKensaMstOuputData outputData)
        {
            Result.Data = new GetListKensaMstResponse(outputData.KensaMsts, outputData.TotalCount);
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
