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

    public SyoukiInfItem(int seqNo, int sortNo, int syoukiKbn, int syoukiKbnStartYm, string syouki, bool isDeleted)
    {
        SeqNo = seqNo;
        SortNo = sortNo;
        SyoukiKbn = syoukiKbn;
        SyoukiKbnStartYm = syoukiKbnStartYm;
        Syouki = syouki;
        IsDeleted = isDeleted;
        PtId = 0;
        SinYm = 0;
        HokenId = 0;
    }

    public long PtId { get; private set; }

    public int SinYm { get; private set; }

    public int HokenId { get; private set; }

    public int SeqNo { get; private set; }

    public int SortNo { get; private set; }

    public int SyoukiKbn { get; private set; }

    public int SyoukiKbnStartYm { get; private set; }

    public string Syouki { get; private set; }

    public bool IsDeleted { get; private set; }
}
