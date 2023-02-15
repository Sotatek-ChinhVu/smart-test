using Domain.Models.Receipt;

namespace UseCase.Receipt;

public class SyobyoKeikaItem
{
    public SyobyoKeikaItem(int sinDay, int seqNo, string keika, bool isDeleted)
    {
        SinDay = sinDay;
        SeqNo = seqNo;
        Keika = keika;
        IsDeleted = isDeleted;
        PtId = 0;
        SinYm = 0;
        HokenId = 0;
    }

    public SyobyoKeikaItem(SyobyoKeikaModel model)
    {
        PtId = model.PtId;
        SinYm = model.SinYm;
        SinDay = model.SinDay;
        HokenId = model.HokenId;
        SeqNo = model.SeqNo;
        Keika = model.Keika;
        IsDeleted = false;
    }

    public long PtId { get; private set; }

    public int SinYm { get; private set; }

    public int HokenId { get; private set; }

    public int SinDay { get; private set; }

    public int SeqNo { get; private set; }

    public string Keika { get; private set; }

    public bool IsDeleted { get; private set; }
}
