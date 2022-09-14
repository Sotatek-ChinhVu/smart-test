using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.MstItem;
using UseCase.MstItem.UpdateAdopted;

namespace EmrCloudApi.Tenant.Presenters.MstItem
{
    public class UpdateAdoptedTenItemPresenter : IUpdateAdoptedTenItemOutputPort
    {
        public Response<UpdateAdoptedTenItemResponse> Result { get; private set; } = default!;

        public void Complete(UpdateAdoptedTenItemOutputData outputData)
        {
            Result = new Response<UpdateAdoptedTenItemResponse>
            {
                Data = new UpdateAdoptedTenItemResponse(outputData.StatusUpdate),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case UpdateAdoptedTenItemStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case UpdateAdoptedTenItemStatus.InvalidValueAdopted:
                    Result.Message = ResponseMessage.InvalidValueAdopted;
                    break;
                case UpdateAdoptedTenItemStatus.InvalidItemCd:
                    Result.Message = ResponseMessage.InvalidItemCd;
                    break;
                case UpdateAdoptedTenItemStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
            }
        }
    }
}
