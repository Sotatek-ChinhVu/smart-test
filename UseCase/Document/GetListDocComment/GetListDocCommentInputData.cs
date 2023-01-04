using UseCase.Core.Sync.Core;

namespace UseCase.Document.GetListDocComment;

public class GetListDocCommentInputData : IInputData<GetListDocCommentOutputData>
{
    public GetListDocCommentInputData(List<string> listReplaceWord)
    {
        ListReplaceWord = listReplaceWord;
    }

    public List<string> ListReplaceWord { get; private set; }
}
