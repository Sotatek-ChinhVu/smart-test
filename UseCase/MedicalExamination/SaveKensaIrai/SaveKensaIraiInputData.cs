using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.SaveKensaIrai;

public class SaveKensaIraiInputData : IInputData<SaveKensaIraiOutputData>
{
    public SaveKensaIraiInputData(int hpId, int userId, long ptId, int sinDate, long raiinNo)
    {
        HpId = hpId;
        UserId = userId;
        PtId = ptId;
        SinDate = sinDate;
        RaiinNo = raiinNo;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public long PtId { get; private set; }

    public int SinDate { get; private set; }

    public long RaiinNo { get; private set; }
}
