using Domain.Models.Receipt;

namespace UseCase.Receipt;

public class SyoukiInfItem
{
    public SyoukiInfItem(SyoukiInfModel model)
    {
        PtId = model.PtId;
        SinYm = model.SinYm;
        HokenId = model.HokenId;
        SeqNo = model.SeqNo;
        SortNo = model.SortNo;
        SyoukiKbn = model.SyoukiKbn;
        Syouki = model.Syouki;
        IsDeleted = false;
    }

    public long PtId { get; private set; }

    public int SinYm { get; private set; }

    public int HokenId { get; private set; }

    public int SeqNo { get; private set; }

    public int SortNo { get; private set; }

    public int SyoukiKbn { get; private set; }

    public string Syouki { get; private set; }

    public bool IsDeleted { get; private set; }
}
