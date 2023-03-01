using UseCase.Core.Sync.Core;

namespace UseCase.Document.DownloadDocumentTemplate;

public class DownloadDocumentTemplateInputData : IInputData<DownloadDocumentTemplateOutputData>
{
    public DownloadDocumentTemplateInputData(int hpId, int userId, long ptId, int sinDate, long raiinNo, int hokenPId, string linkFile, List<ReplaceCommentInputItem> listReplaceComments)
    {
        HpId = hpId;
        UserId = userId;
        PtId = ptId;
        SinDate = sinDate;
        RaiinNo = raiinNo;
        HokenPId = hokenPId;
        LinkFile = linkFile;
        ListReplaceComments = listReplaceComments;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public long PtId { get; private set; }

    public int SinDate { get; private set; }

    public long RaiinNo { get; private set; }

    public int HokenPId { get; private set; }

    public string LinkFile { get; private set; }

    public List<ReplaceCommentInputItem> ListReplaceComments { get; private set; }
}
