using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.NextOrder;
using UseCase.NextOrder.CheckNextOrdHaveOdr;

namespace EmrCloudApi.Presenters.NextOrder
{
    public class CheckNextOrdHaveOrdPresenter : ICheckNextOrdHaveOdrOutputPort
    {
        public Response<CheckNextOrdHaveOrdResponse> Result { get; private set; } = default!;

        public void Complete(CheckNextOrdHaveOdrOutputData outputData)
        {
            Result = new Response<CheckNextOrdHaveOrdResponse>()
            {
                Data = new CheckNextOrdHaveOrdResponse(outputData.Result),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case CheckNextOrdHaveOdrStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case CheckNextOrdHaveOdrStatus.InvalidPtId:
                    Result.Message = ResponseMessage.InvalidPtId;
                    break;
                case CheckNextOrdHaveOdrStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidRsvkrtKbn;
                    break;
                case CheckNextOrdHaveOdrStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
            }

        }
    }
}
