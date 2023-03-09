using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MedicalExamination;
using UseCase.MedicalExamination.CheckedExpired;

namespace EmrCloudApi.Presenters.MedicalExamination
{
    public class CheckedExpiredPresenter : ICheckedExpiredOutputPort
    {
        public Response<CheckedExpiredResponse> Result { get; private set; } = default!;

        public void Complete(CheckedExpiredOutputData outputData)
        {
            Result = new Response<CheckedExpiredResponse>()
            {
                Data = new CheckedExpiredResponse(outputData.Result),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case CheckedExpiredStatus.InValidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case CheckedExpiredStatus.InValidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case CheckedExpiredStatus.InputNotData:
                    Result.Message = ResponseMessage.InputNoData;
                    break;
                case CheckedExpiredStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
            }
        }
    }
}
