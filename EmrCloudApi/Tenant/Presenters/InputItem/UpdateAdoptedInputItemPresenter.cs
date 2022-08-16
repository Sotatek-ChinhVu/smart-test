using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.InputItem;
using UseCase.InputItem.UpdateAdopted;

namespace EmrCloudApi.Tenant.Presenters.InputItem
{
    public class UpdateAdoptedInputItemPresenter : IUpdateAdoptedInputItemOutputPort
    {
        public Response<UpdateAdoptedInputItemResponse> Result { get; private set; } = default!;

        public void Complete(UpdateAdoptedInputItemOutputData outputData)
        {
            Result = new Response<UpdateAdoptedInputItemResponse>
            {
                Data = new UpdateAdoptedInputItemResponse(outputData.StatusUpdate),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case UpdateAdoptedInputItemStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case UpdateAdoptedInputItemStatus.InvalidValueAdopted:
                    Result.Message = ResponseMessage.InvalidValueAdopted;
                    break;
                case UpdateAdoptedInputItemStatus.InvalidItemCd:
                    Result.Message = ResponseMessage.InvalidItemCd;
                    break;
                case UpdateAdoptedInputItemStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
            }
        }
    }
}
