using EmrCloudApi.Constants;
using EmrCloudApi.Responses.TimeZoneConf;
using EmrCloudApi.Responses;
using UseCase.TimeZoneConf.SaveTimeZoneConf;

namespace EmrCloudApi.Presenters.TimeZoneConf
{
    public class SaveTimeZoneConfPresenter : ISaveTimeZoneConfOutputPort
    {
        public Response<SaveTimeZoneConfResponse> Result { get; private set; } = default!;

        public void Complete(SaveTimeZoneConfOutputData outputData)
        {
            Result = new Response<SaveTimeZoneConfResponse>()
            {
                Data = new SaveTimeZoneConfResponse(outputData.Status),
                Status = (int)outputData.Status
            };
            switch (outputData.Status)
            {
                case SaveTimeZoneConfStatus.Successful:
                    Result.Message = ResponseMessage.Success;
                    break;
                case SaveTimeZoneConfStatus.NoData:
                    Result.Message = ResponseMessage.NoData;
                    break;
                case SaveTimeZoneConfStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case SaveTimeZoneConfStatus.InvalidUserId:
                    Result.Message = ResponseMessage.InvalidUserId;
                    break;
                case SaveTimeZoneConfStatus.Failed:
                    Result.Message = ResponseMessage.Failed;
                    break;
                case SaveTimeZoneConfStatus.NotHavePermission:
                    Result.Message = ResponseMessage.NoPermission;
                    break;
            }
        }
    }
}
