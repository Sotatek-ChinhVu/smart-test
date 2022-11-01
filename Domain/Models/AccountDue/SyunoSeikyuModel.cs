namespace Domain.Models.AccountDue;

public class SyunoSeikyuModel
{
    public SyunoSeikyuModel(int hpId, long ptId, int sinDate, long raiinNo, int nyukinKbn, int seikyuTensu, int adjustFutan, int seikyuGaku, string seikyuDetail, int newSeikyuTensu, int newAdjustFutan, int newSeikyuGaku, string newSeikyuDetail)
    {
        HpId = hpId;
        PtId = ptId;
        SinDate = sinDate;
        RaiinNo = raiinNo;
        NyukinKbn = nyukinKbn;
        SeikyuTensu = seikyuTensu;
        AdjustFutan = adjustFutan;
        SeikyuGaku = seikyuGaku;
        SeikyuDetail = seikyuDetail;
        NewSeikyuTensu = newSeikyuTensu;
        NewAdjustFutan = newAdjustFutan;
        NewSeikyuGaku = newSeikyuGaku;
        NewSeikyuDetail = newSeikyuDetail;
    }

    public int HpId { get; set; }
    
    public long PtId { get; set; }
    
    public int SinDate { get; set; }
    
    public long RaiinNo { get; set; }
    
    public int NyukinKbn { get; set; }
    
    public int SeikyuTensu { get; set; }
    
    public int AdjustFutan { get; set; }
    
    public int SeikyuGaku { get; set; }
    
    public string SeikyuDetail { get; set; }
    
    public int NewSeikyuTensu { get; set; }
    
    public int NewAdjustFutan { get; set; }
    
    public int NewSeikyuGaku { get; set; }
    
    public string NewSeikyuDetail { get; set; }
}
