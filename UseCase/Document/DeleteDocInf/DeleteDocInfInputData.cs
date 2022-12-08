using UseCase.Core.Sync.Core;

namespace UseCase.Document.DeleteDocInf;

public class DeleteDocInfInputData : IInputData<DeleteDocInfOutputData>
{
    public DeleteDocInfInputData(int hpId, int userId, long ptId, int sinDate, long raiinNo, int seqNo)
    {
        HpId = hpId;
        UserId = userId;
        PtId = ptId;
        SinDate = sinDate;
        RaiinNo = raiinNo;
        SeqNo = seqNo;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public long PtId { get; private set; }

    public int SinDate { get; private set; }

    public long RaiinNo { get; private set; }

    public int SeqNo { get; private set; }

}
