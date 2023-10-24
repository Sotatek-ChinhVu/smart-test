using EmrCloudApi.Responses.MstItem;
using EmrCloudApi.Responses;
using UseCase.MstItem.UpdateByomeiMst;
using EmrCloudApi.Constants;

namespace EmrCloudApi.Presenters.MstItem
{
    public class UpdateByomeiMstPresenter : IUpdateByomeiMstOutputPort
    {
        public Response<UpdateByomeiMstResponse> Result { get; private set; } = default!;

        public void Complete(UpdateByomeiMstOutputData outputData)
        {
            Result = new Response<UpdateByomeiMstResponse>
            {
                Data = new UpdateByomeiMstResponse(outputData.CheckResult),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case UpdateByomeiMstStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case UpdateByomeiMstStatus.InValidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case UpdateByomeiMstStatus.InValidUserId:
                    Result.Message = ResponseMessage.InvalidUserId;
                    break;
                case UpdateByomeiMstStatus.InvalidDataUpdate:
                    Result.Message = ResponseMessage.InputDataNull;
                    break;
            }
        }
    }
}
