using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MedicalExamination;
using UseCase.MedicalExamination.Calculate;

namespace EmrCloudApi.Presenters.MedicalExamination
{
    public class CalculatePresenter : ICalculateOutputPort
    {
        public Response<CalculateResponse> Result { get; private set; } = default!;

        public void Complete(CalculateOutputData outputData)
        {
            Result = new Response<CalculateResponse>()
            {
                Data = new CalculateResponse(outputData.PtId, outputData.SinDate),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case CalculateStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case CalculateStatus.Failed:
                    Result.Message = ResponseMessage.Failed;
                    break;
            }
        }
    }
}
