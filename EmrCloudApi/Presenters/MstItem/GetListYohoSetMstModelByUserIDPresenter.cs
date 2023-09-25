using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MstItem;
using UseCase.MstItem.GetListYohoSetMstModelByUserID;

namespace EmrCloudApi.Presenters.MstItem
{
    public class GetListYohoSetMstModelByUserIDPresenter : IGetListYohoSetMstModelByUserIDOutputPort
    {
        public Response<GetListYohoSetMstModelByUserIDResponse> Result { get; private set; } = new();

        public void Complete(GetListYohoSetMstModelByUserIDOutputData output)
        {
            Result.Data = new GetListYohoSetMstModelByUserIDResponse(output.YohoSetMsts);
            Result.Message = GetMessage(output.Status);
            Result.Status = (int)output.Status;
        }

        private string GetMessage(GetListYohoSetMstModelByUserIDStatus status) => status switch
        {
            GetListYohoSetMstModelByUserIDStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            GetListYohoSetMstModelByUserIDStatus.NoData => ResponseMessage.NoData,
            GetListYohoSetMstModelByUserIDStatus.Successful => ResponseMessage.Success,
            _ => string.Empty
        };
    }
}
