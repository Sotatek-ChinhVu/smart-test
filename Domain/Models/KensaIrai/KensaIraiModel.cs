namespace Domain.Models.KensaIrai;

public class KensaIraiModel
{
    public KensaIraiModel(int sinDate, long raiinNo, long iraiCd, long ptId, long ptNum, string name, string kanaName, int sex, int birthday, int tosekiKbn, int sikyuKbn, List<KensaIraiDetailModel> kensaIraiDetails)
    {
        SinDate = sinDate;
        RaiinNo = raiinNo;
        IraiCd = iraiCd;
        PtId = ptId;
        PtNum = ptNum;
        Name = name;
        KanaName = kanaName;
        Sex = sex;
        Birthday = birthday;
        TosekiKbn = tosekiKbn;
        SikyuKbn = sikyuKbn;
        KensaIraiDetails = kensaIraiDetails;
    }

    public KensaIraiModel(int sinDate, long raiinNo, long iraiCd, long ptId, long ptNum, string name, string kanaName, int sex, int birthday, int tosekiKbn, int sikyuKbn, DateTime updateDate, List<KensaIraiDetailModel> kensaIraiDetails)
    {
        SinDate = sinDate;
        RaiinNo = raiinNo;
        IraiCd = iraiCd;
        PtId = ptId;
        PtNum = ptNum;
        Name = name;
        KanaName = kanaName;
        Sex = sex;
        Birthday = birthday;
        TosekiKbn = tosekiKbn;
        SikyuKbn = sikyuKbn;
        UpdateDate = updateDate;
        KensaIraiDetails = kensaIraiDetails;
    }

    public KensaIraiModel(int sinDate, long raiinNo, long iraiCd, long ptId, long ptNum, string name, string kanaName, int sex, int birthday, int tosekiKbn, int sikyuKbn, int kaId, List<KensaIraiDetailModel> kensaIraiDetails)
    {
        SinDate = sinDate;
        RaiinNo = raiinNo;
        IraiCd = iraiCd;
        PtId = ptId;
        PtNum = ptNum;
        Name = name;
        KanaName = kanaName;
        Sex = sex;
        Birthday = birthday;
        TosekiKbn = tosekiKbn;
        SikyuKbn = sikyuKbn;
        KaId = kaId;
        KensaIraiDetails = kensaIraiDetails;
    }

    public KensaIraiModel(int sinDate, long raiinNo, long iraiCd, long ptId, long ptNum, string name, string kanaName, int sex, int birthday, int tosekiKbn, int sikyuKbn, int kaId, DateTime updateDate, List<KensaIraiDetailModel> kensaIraiDetails)
    {
        SinDate = sinDate;
        RaiinNo = raiinNo;
        IraiCd = iraiCd;
        PtId = ptId;
        PtNum = ptNum;
        Name = name;
        KanaName = kanaName;
        Sex = sex;
        Birthday = birthday;
        TosekiKbn = tosekiKbn;
        SikyuKbn = sikyuKbn;
        KaId = kaId;
        UpdateDate = updateDate;
        KensaIraiDetails = kensaIraiDetails;
    }

    public KensaIraiModel UpdateIraiCd(long iraiCd, int kaId)
    {
        IraiCd = iraiCd;
        KaId = kaId;
        return this;
    }

    public int SinDate { get; private set; }

    public long RaiinNo { get; private set; }

    public long IraiCd { get; private set; }

    public long PtId { get; private set; }

    public long PtNum { get; private set; }

    public string Name { get; private set; }

    public string KanaName { get; private set; }

    public int Sex { get; private set; }

    public int Birthday { get; private set; }

    public int TosekiKbn { get; private set; }

    public int SikyuKbn { get; private set; }

    public int KaId { get; private set; }

    public DateTime UpdateDate { get; private set; }

    public List<KensaIraiDetailModel> KensaIraiDetails { get; private set; }
}
