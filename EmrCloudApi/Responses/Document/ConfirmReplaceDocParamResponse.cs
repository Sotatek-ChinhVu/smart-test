using EmrCloudApi.Responses.Document.Dto;
using UseCase.Document;

namespace EmrCloudApi.Responses.Document;

public class ConfirmReplaceDocParamResponse
{
    public ConfirmReplaceDocParamResponse(List<DocCommentOutputItem> listDocCommnet)
    {
        ListDocCommnet = listDocCommnet.Select(item => new DocCommentDto(item)).ToList();
    }

    public List<DocCommentDto> ListDocCommnet { get; private set; }
}
