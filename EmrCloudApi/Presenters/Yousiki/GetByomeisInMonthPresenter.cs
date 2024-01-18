using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Yousiki;
using UseCase.Yousiki.GetByomeisInMonth;

namespace EmrCloudApi.Presenters.Yousiki
{
    public class GetByomeisInMonthPresenter : IGetByomeisInMonthOutputPort
    {
        public Response<GetByomeisInMonthResponse> Result { get; private set; } = new();
        public void Complete(GetByomeisInMonthOutputData outputData)
        {
            Result.Data = new GetByomeisInMonthResponse(outputData.ByomeisInMonth);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }

        private string GetMessage(GetByomeisInMonthStatus status) => status switch
        {
            GetByomeisInMonthStatus.Successed => ResponseMessage.Success,
            _ => string.Empty
        };
    }
}
