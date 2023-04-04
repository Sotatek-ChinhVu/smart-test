using Domain.Models.Receipt;

namespace UseCase.Receipt;

public class ReceStatusItem
{
    public ReceStatusItem(long ptId, int seikyuYm, int hokenId, int sinYm, int fusenKbn, bool isPaperRece, bool isOutput, int statusKbn, bool isPrechecked)
    {
        PtId = ptId;
        SeikyuYm = seikyuYm;
        HokenId = hokenId;
        SinYm = sinYm;
        FusenKbn = fusenKbn;
        IsPaperRece = isPaperRece;
        IsOutput = isOutput;
        StatusKbn = statusKbn;
        IsPrechecked = isPrechecked;
    }

    public ReceStatusItem(ReceStatusModel model)
    {
        PtId = model.PtId;
        SeikyuYm = model.SeikyuYm;
        HokenId = model.HokenId;
        SinYm = model.SinYm;
        FusenKbn = model.FusenKbn;
        IsPaperRece = model.IsPaperRece;
        IsOutput = model.IsOutput;
        StatusKbn = model.StatusKbn;
        IsPrechecked = model.IsPrechecked;
    }

    public long PtId { get; private set; }

    public int SeikyuYm { get; private set; }

    public int HokenId { get; private set; }

    public int SinYm { get; private set; }

    public int FusenKbn { get; private set; }

    public bool IsPaperRece { get; private set; }

    public bool IsOutput { get; private set; }

    public int StatusKbn { get; private set; }

    public bool IsPrechecked { get; private set; }
}
