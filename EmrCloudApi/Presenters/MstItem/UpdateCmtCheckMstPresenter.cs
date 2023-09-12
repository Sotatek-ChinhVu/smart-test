using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MstItem;
using UseCase.MstItem.UpdateCmtCheckMst;
using static UseCase.MstItem.UpdateCmtCheckMst.UpdateCmtCheckMstOutputData;

namespace EmrCloudApi.Presenters.MstItem
{
    public class UpdateCmtCheckMstPresenter
    {
        public Response<UpdateCmtCheckMstResponse> Result { get; private set; } = default!;

        public void Complete(UpdateCmtCheckMstOutputData outputData)
        {
            Result = new Response<UpdateCmtCheckMstResponse>
            {
                Data = new UpdateCmtCheckMstResponse(outputData.CheckResult),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case UpdateCmtCheckMstStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case UpdateCmtCheckMstStatus.InValidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case UpdateCmtCheckMstStatus.InValidUserId:
                    Result.Message = ResponseMessage.InvalidUserId;
                    break;
                case UpdateCmtCheckMstStatus.InvalidDataUpdate:
                    Result.Message = ResponseMessage.InputDataNull;
                    break;
            }
        }
    }
}
