using UseCase.Core.Sync.Core;

namespace UseCase.Document.DeleteDocInf;

public class DeleteDocInfInputData : IInputData<DeleteDocInfOutputData>
{
    public DeleteDocInfInputData(int hpId, int userId, long fileId, long ptId)
    {
        HpId = hpId;
        UserId = userId;
        FileId = fileId;
        PtId = ptId;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public long FileId { get; private set; }

    public long PtId { get; private set; }
}
