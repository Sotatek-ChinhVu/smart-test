using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.TimeZoneConf;
using UseCase.TimeZoneConf.GetTimeZoneConfGroup;

namespace EmrCloudApi.Presenters.TimeZoneConf
{
    public class GetTimeZoneConfGroupPresenter : IGetTimeZoneConfGroupOutputPort
    {
        public Response<GetTimeZoneConfGroupResponse> Result { get; private set; } = default!;

        public void Complete(GetTimeZoneConfGroupOutputData outputData)
        {
            Result = new Response<GetTimeZoneConfGroupResponse>()
            {
                Data = new GetTimeZoneConfGroupResponse(outputData.TimeZoneConfGroups, outputData.IsHavePermission),
                Status = (int)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetTimeZoneConfGroupStatus.Successful:
                    Result.Message = ResponseMessage.Success;
                    break;
                case GetTimeZoneConfGroupStatus.NoData:
                    Result.Message = ResponseMessage.NoData;
                    break;
                case GetTimeZoneConfGroupStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
            }
        }
    }
}
