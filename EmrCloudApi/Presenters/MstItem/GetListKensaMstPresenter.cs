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
            Result.Data = new GetListKensaMstResponse(outputData.KensaMsts);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }

        private string GetMessage(SearchTenMstItemStatus status) => status switch
        {
            SearchTenMstItemStatus.Successed => ResponseMessage.Success,
            SearchTenMstItemStatus.InValidHpId => ResponseMessage.InvalidHpId,
            _ => string.Empty
        };
    }
}
