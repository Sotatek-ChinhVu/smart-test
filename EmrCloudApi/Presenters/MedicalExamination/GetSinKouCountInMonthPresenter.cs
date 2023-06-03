using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MedicalExamination;
using UseCase.MedicalExamination.GetSinkouCountInMonth;

namespace EmrCloudApi.Presenters.MedicalExamination
{
    public class GetSinKouCountInMonthPresenter : IGetSinkouCountInMonthOutputPort
    {
        public Response<GetSinkouCountInMonthResponse> Result { get; private set; } = default!;

        public void Complete(GetSinkouCountInMonthOutputData outputData)
        {
            Result = new Response<GetSinkouCountInMonthResponse>()
            {
                Data = new GetSinkouCountInMonthResponse(outputData.SinKouiCounts),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetSinkouCountInMonthStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case GetSinkouCountInMonthStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case GetSinkouCountInMonthStatus.InvalidPtId:
                    Result.Message = ResponseMessage.InvalidPtId;
                    break;
                case GetSinkouCountInMonthStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
            }
        }
    }
}
