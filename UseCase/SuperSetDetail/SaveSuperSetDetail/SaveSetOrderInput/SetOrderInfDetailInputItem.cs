namespace UseCase.SuperSetDetail.SaveSuperSetDetail.SaveSetOrderInput;

public class SetOrderInfDetailInputItem
{
    public SetOrderInfDetailInputItem(int sinKouiKbn, string itemCd, string itemName, double suryo, string unitName, int unitSBT, double termVal, int kohatuKbn, int syohoKbn, int syohoLimitKbn, int drugKbn, int yohoKbn, string kokuji1, string kokuji2, int isNodspRece, string ipnCd, string ipnName, string bunkatu, string cmtName, string cmtOpt, string fontColor, int commentNewline)
    {
        SinKouiKbn = sinKouiKbn;
        ItemCd = itemCd;
        ItemName = itemName;
        Suryo = suryo;
        UnitName = unitName;
        UnitSBT = unitSBT;
        TermVal = termVal;
        KohatuKbn = kohatuKbn;
        SyohoKbn = syohoKbn;
        SyohoLimitKbn = syohoLimitKbn;
        DrugKbn = drugKbn;
        YohoKbn = yohoKbn;
        Kokuji1 = kokuji1;
        Kokuji2 = kokuji2;
        IsNodspRece = isNodspRece;
        IpnCd = ipnCd;
        IpnName = ipnName;
        Bunkatu = bunkatu;
        CmtName = cmtName;
        CmtOpt = cmtOpt;
        FontColor = fontColor;
        CommentNewline = commentNewline;
    }

    public int SinKouiKbn { get; private set; }

    public string ItemCd { get; private set; }

    public string ItemName { get; private set; }

    public double Suryo { get; private set; }

    public string UnitName { get; private set; }

    public int UnitSBT { get; private set; }

    public double TermVal { get; private set; }

    public int KohatuKbn { get; private set; }

    public int SyohoKbn { get; private set; }

    public int SyohoLimitKbn { get; private set; }

    public int DrugKbn { get; private set; }

    public int YohoKbn { get; private set; }

    public string Kokuji1 { get; private set; }

    public string Kokuji2 { get; private set; }

    public int IsNodspRece { get; private set; }

    public string IpnCd { get; private set; }

    public string IpnName { get; private set; }

    public string Bunkatu { get; private set; }

    public string CmtName { get; private set; }

    public string CmtOpt { get; private set; }

    public string FontColor { get; private set; }

    public int CommentNewline { get; private set; }
}
