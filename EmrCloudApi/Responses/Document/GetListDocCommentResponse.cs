using UseCase.Document;

namespace EmrCloudApi.Responses.Document;

public class GetListDocCommentResponse
{
    public GetListDocCommentResponse(List<DocCommentOutputItem> listDocCommnet)
    {
        ListDocCommnet = listDocCommnet.Select(item => new DocCommentDto(item)).ToList();
    }

    public List<DocCommentDto> ListDocCommnet { get; private set; }
}
