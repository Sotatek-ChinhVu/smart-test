using UseCase.Receipt;

namespace EmrCloudApi.Responses.Receipt;

public class GetReceStatusResponse
{
    public GetReceStatusResponse(ReceStatusItem output)
    {
        PtId = output.PtId;
        SeikyuYm = output.SeikyuYm;
        HokenId = output.HokenId;
        SinYm = output.SinYm;
        FusenKbn = output.FusenKbn;
        IsPaperRece = output.IsPaperRece;
        StatusKbn = output.StatusKbn;
        IsPrechecked = output.IsPrechecked;
    }

    public long PtId { get; private set; }

    public int SeikyuYm { get; private set; }

    public int HokenId { get; private set; }

    public int SinYm { get; private set; }

    public int FusenKbn { get; private set; }

    public bool IsPaperRece { get; private set; }

    public int StatusKbn { get; private set; }

    public bool IsPrechecked { get; private set; }
}
