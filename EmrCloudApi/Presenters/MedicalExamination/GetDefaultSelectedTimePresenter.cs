using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MedicalExamination;
using UseCase.MedicalExamination.GetDefaultSelectedTime;

namespace EmrCloudApi.Presenters.MedicalExamination
{
    public class GetDefaultSelectedTimePresenter : IGetDefaultSelectedTimeOutputPort
    {
        public Response<GetDefaultSelectedTimeResponse> Result { get; private set; } = default!;

        public void Complete(GetDefaultSelectedTimeOutputData outputData)
        {
            Result = new Response<GetDefaultSelectedTimeResponse>()
            {
                Data = new GetDefaultSelectedTimeResponse(outputData.Value, outputData.Message, outputData.TimeKbnForChild, outputData.CurrentTimeKbn, outputData.BeforeTimeKbn),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetDefaultSelectedTimeStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case GetDefaultSelectedTimeStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case GetDefaultSelectedTimeStatus.InvalidUketukeTime:
                    Result.Message = ResponseMessage.InvalidUketukeTime;
                    break;
                case GetDefaultSelectedTimeStatus.InvalidBirthDay:
                    Result.Message = ResponseMessage.InvalidBirthDay;
                    break;
                case GetDefaultSelectedTimeStatus.InvalidDayOfWeek:
                    Result.Message = ResponseMessage.InvalidDayOfWeek;
                    break;
            }
        }
    }
}
