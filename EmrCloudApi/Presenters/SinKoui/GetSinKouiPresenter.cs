using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.SinKoui;
using UseCase.SinKoui.GetSinKoui;

namespace EmrCloudApi.Presenters.SinKoui
{
    public class GetSinKouiPresenter : IGetListSinKouiOutputPort
    {
        public Response<GetSinKouiResponse> Result { get; private set; } = new Response<GetSinKouiResponse>();

        public void Complete(GetListSinKouiOutputData output)
        {
            Result.Data = new GetSinKouiResponse(output.SinYmBindings);
            Result.Message = GetMessage(output.Status);
            Result.Status = (int)output.Status;
        }

        private string GetMessage(GetListSinKouiStatus status) => status switch
        {
            GetListSinKouiStatus.Success => ResponseMessage.Success,
            GetListSinKouiStatus.InvalidPtId => ResponseMessage.InvalidPtId,
            _ => string.Empty
        };
    }
}
