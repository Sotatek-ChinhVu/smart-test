using UseCase.KarteInfs.GetLists;

namespace EmrCloudApi.Responses.KarteInf;

public class KarteInfDto
{
    public KarteInfDto(GetListKarteInfOuputItem model)
    {
        HpId = model.HpId;
        RaiinNo = model.RaiinNo;
        KarteKbn = model.KarteKbn;
        SeqNo = model.SeqNo;
        PtId = model.PtId;
        SinDate = model.SinDate;
        Text = model.Text;
        IsDeleted = model.IsDeleted;
        RichText = model.RichText;
    }

    public int HpId { get; private set; }

    public long RaiinNo { get; private set; }

    public int KarteKbn { get; private set; }

    public long SeqNo { get; private set; }

    public long PtId { get; private set; }

    public int SinDate { get; private set; }

    public string Text { get; private set; }

    public int IsDeleted { get; private set; }

    public string RichText { get; private set; }
}
