using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Document;
using UseCase.Document.GetListDocComment;

namespace EmrCloudApi.Presenters.Document;

public class GetListDocCommentPresenter : IGetListDocCommentOutputPort
{
    public Response<GetListDocCommentResponse> Result { get; private set; } = new();

    public void Complete(GetListDocCommentOutputData output)
    {
        Result.Data = new GetListDocCommentResponse(output.DocComments);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetListDocCommentStatus status) => status switch
    {
        GetListDocCommentStatus.Successed => ResponseMessage.Success,
        GetListDocCommentStatus.Failed => ResponseMessage.Failed,
        _ => string.Empty
    };
}
