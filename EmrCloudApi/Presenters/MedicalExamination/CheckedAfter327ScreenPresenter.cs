using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MedicalExamination;
using UseCase.MedicalExamination.CheckedAfter327Screen;

namespace EmrCloudApi.Presenters.MedicalExamination
{
    public class CheckedAfter327ScreenPresenter : ICheckedAfter327ScreenOutputPort
    {
        public Response<CheckedAfter327ScreenResponse> Result { get; private set; } = default!;

        public void Complete(CheckedAfter327ScreenOutputData outputData)
        {

            Result = new Response<CheckedAfter327ScreenResponse>()
            {
                Data = new CheckedAfter327ScreenResponse(outputData.Message, outputData.SinKouiCountModels),
                Status = (byte)outputData.Status
            };

            switch (outputData.Status)
            {
                case CheckedAfter327ScreenStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case CheckedAfter327ScreenStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case CheckedAfter327ScreenStatus.InvalidPtId:
                    Result.Message = ResponseMessage.InvalidPtId;
                    break;
                case CheckedAfter327ScreenStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case CheckedAfter327ScreenStatus.Failed:
                    Result.Message = ResponseMessage.Failed;
                    break;
            }
        }
    }
}
