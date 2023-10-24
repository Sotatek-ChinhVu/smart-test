using EmrCloudApi.Constants;
using EmrCloudApi.Responses.MstItem;
using EmrCloudApi.Responses;
using static UseCase.MstItem.UpdateCmtCheckMst.UpdateCmtCheckMstOutputData;
using UseCase.MstItem.UpdateCmtCheckMst;
using EmrCloudApi.Responses.Diseases;
using UseCase.ListSetMst.UpdateListSetMst;
using UseCase.ByomeiSetMst.UpdateByomeiSetMst;

namespace EmrCloudApi.Presenters.Diseases
{
    public class UpdateByomeiSetMstPresenter
    {
        public Response<UpdateByomeiSetMstResponse> Result { get; private set; } = default!;

        public void Complete(UpdateByomeiSetMstOutputData outputData)
        {
            Result = new Response<UpdateByomeiSetMstResponse>
            {
                Data = new UpdateByomeiSetMstResponse(outputData.CheckResult),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case UpdateByomeiSetMstStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case UpdateByomeiSetMstStatus.InValidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case UpdateByomeiSetMstStatus.InValidUserId:
                    Result.Message = ResponseMessage.InvalidUserId;
                    break;
                case UpdateByomeiSetMstStatus.InvalidDataUpdate:
                    Result.Message = ResponseMessage.InputDataNull;
                    break;
            }
        }
    }
}
