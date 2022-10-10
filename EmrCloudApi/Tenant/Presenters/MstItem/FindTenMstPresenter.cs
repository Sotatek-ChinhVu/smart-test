using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.MstItem;
using UseCase.MstItem.FindTenMst;

namespace EmrCloudApi.Tenant.Presenters.MstItem
{
    public class FindTenMstPresenter : IFindTenMstOutputPort
    {
        public Response<FindtenMstResponse> Result { get; private set; } = default!;

        public void Complete(FindTenMstOutputData outputData)
        {
            Result = new Response<FindtenMstResponse>
            {
                Data = new FindtenMstResponse(outputData.TenItemModel),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case FindTenMstStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case FindTenMstStatus.InValidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case FindTenMstStatus.InvalidSindate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case FindTenMstStatus.InvalidItemCd:
                    Result.Message = ResponseMessage.InvalidItemCd;
                    break;
                case FindTenMstStatus.Failed:
                    Result.Message = ResponseMessage.Failed;
                    break;
            }
        }
    }
}
