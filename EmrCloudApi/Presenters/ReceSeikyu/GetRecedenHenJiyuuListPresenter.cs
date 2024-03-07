using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.ReceSeikyu;
using UseCase.ReceSeikyu.GetRecedenHenJiyuuList;

namespace EmrCloudApi.Presenters.ReceSeikyu;

public class GetRecedenHenJiyuuListPresenter : IGetRecedenHenJiyuuListOutputPort
{
    public Response<GetRecedenHenJiyuuListResponse> Result { get; private set; } = new Response<GetRecedenHenJiyuuListResponse>();

    public void Complete(GetRecedenHenJiyuuListOutputData output)
    {
        Result.Data = new GetRecedenHenJiyuuListResponse(output.RecedenHenJiyuuModelList.Select(item => new RecedenHenJiyuuDto(item)).ToList());
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetRecedenHenJiyuuListStatus status) => status switch
    {
        GetRecedenHenJiyuuListStatus.Successed => ResponseMessage.Success,
        GetRecedenHenJiyuuListStatus.Failled => ResponseMessage.Failed,
        _ => string.Empty
    };
}
