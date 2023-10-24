using EmrCloudApi.Constants;
using EmrCloudApi.Responses.MstItem;
using EmrCloudApi.Responses;
using static UseCase.MstItem.UpdateCmtCheckMst.UpdateCmtCheckMstOutputData;
using UseCase.MstItem.UpdateCmtCheckMst;
using UseCase.ListSetMst.UpdateListSetMst;
using EmrCloudApi.Responses.NewFolder;

namespace EmrCloudApi.Presenters.ListSetMst
{
    public class UpdateListSetMstPresenter
    {
        public Response<UpdateListSetMstResponse> Result { get; private set; } = default!;

        public void Complete(UpdateListSetMstOutputData outputData)
        {
            Result = new Response<UpdateListSetMstResponse>
            {
                Data = new UpdateListSetMstResponse(outputData.CheckResult),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case UpdateListSetMstStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case UpdateListSetMstStatus.InValidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case UpdateListSetMstStatus.InValidUserId:
                    Result.Message = ResponseMessage.InvalidUserId;
                    break;
                case UpdateListSetMstStatus.InvalidDataUpdate:
                    Result.Message = ResponseMessage.InputDataNull;
                    break;
            }
        }
    }
}
