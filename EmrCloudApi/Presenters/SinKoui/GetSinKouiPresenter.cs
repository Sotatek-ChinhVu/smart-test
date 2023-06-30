using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.SinKoui;
using UseCase.SinKoui.GetSinKoui;

namespace EmrCloudApi.Presenters.SinKoui
{
    public class GetSinKouiPresenter : IGetSinKouiListOutputPort
    {
        public Response<GetSinKouiResponse> Result { get; private set; } = new Response<GetSinKouiResponse>();

        public void Complete(GetSinKouiListOutputData output)
        {
            Result.Data = new GetSinKouiResponse(output.SinYmBindings);
            Result.Message = GetMessage(output.Status);
            Result.Status = (int)output.Status;
        }

        private string GetMessage(GetSinKouiListStatus status) => status switch
        {
            GetSinKouiListStatus.Success => ResponseMessage.Success,
            GetSinKouiListStatus.InvalidPtId => ResponseMessage.InvalidPtId,
            _ => string.Empty
        };
    }
}
