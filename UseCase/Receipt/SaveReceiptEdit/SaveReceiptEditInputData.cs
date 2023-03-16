using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.SaveReceiptEdit;

public class SaveReceiptEditInputData : IInputData<SaveReceiptEditOutputData>
{
    public SaveReceiptEditInputData(int hpId, int userId, int seikyuYm, long ptId, int sinYm, int hokenId, ReceiptEditItem receiptEdit)
    {
        HpId = hpId;
        UserId = userId;
        SeikyuYm = seikyuYm;
        PtId = ptId;
        SinYm = sinYm;
        HokenId = hokenId;
        ReceiptEdit = receiptEdit;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public int SeikyuYm { get; private set; }

    public long PtId { get; private set; }

    public int SinYm { get; private set; }

    public int HokenId { get; private set; }

    public ReceiptEditItem ReceiptEdit { get; private set; }
}
