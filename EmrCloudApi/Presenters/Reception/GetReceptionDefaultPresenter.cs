using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Reception;
using UseCase.Reception.GetReceptionDefault;

namespace EmrCloudApi.Presenters.Reception
{
    public class GetReceptionDefaultPresenter : IGetReceptionDefaultOutputPort
    {
        public Response<GetReceptionDefaultResponse> Result { get; private set; } = default!;

        public void Complete(GetReceptionDefaultOutputData outputData)
        {
            Result = new Response<GetReceptionDefaultResponse>()
            {
                Data = new GetReceptionDefaultResponse(outputData.Data),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetReceptionDefaultStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case GetReceptionDefaultStatus.InvalidPtId:
                    Result.Message = ResponseMessage.InvalidPtId;
                    break;
                case GetReceptionDefaultStatus.InvalidSindate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case GetReceptionDefaultStatus.InvalidDefautDoctorSetting:
                    Result.Message = ResponseMessage.InvalidDefaultSettingDoctor;
                    break;
                case GetReceptionDefaultStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case GetReceptionDefaultStatus.Failed:
                    Result.Message = ResponseMessage.Failed;
                    break;
            }
        }
    }
}
