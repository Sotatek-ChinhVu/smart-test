using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MstItem;
using UseCase.MstItem.UpdateAdoptedByomei;

namespace EmrCloudApi.Presenters.MstItem
{
    public class UpdateAdoptedByomeiPresenter : IUpdateAdoptedByomeiOutputPort
    {
        public Response<UpdateAdoptedTenItemResponse> Result { get; private set; } = default!;

        public void Complete(UpdateAdoptedByomeiOutputData outputData)
        {
            Result = new Response<UpdateAdoptedTenItemResponse>
            {
                Data = new UpdateAdoptedTenItemResponse(outputData.StatusUpdate),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case UpdateAdoptedByomeiStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case UpdateAdoptedByomeiStatus.InvalidHospitalId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case UpdateAdoptedByomeiStatus.InvalidByomeiCd:
                    Result.Message = ResponseMessage.InvalidItemCd;
                    break;
                case UpdateAdoptedByomeiStatus.Failed:
                    Result.Message = ResponseMessage.Failed;
                    break;
            }
        }
    }
}
