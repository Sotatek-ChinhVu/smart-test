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

    public SaveSuperSetDetailStatus Validate()
    {
        if (SinKouiKbn < 0)
        {
            return SaveSuperSetDetailStatus.InvalidSetOrderSinKouiKbn;
        }
        else if (ItemCd.Length > 10)
        {
            return SaveSuperSetDetailStatus.ItemCdMaxLength10;
        }
        else if (ItemName.Length > 240)
        {
            return SaveSuperSetDetailStatus.ItemNameMaxLength240;
        }
        else if (UnitName.Length > 24)
        {
            return SaveSuperSetDetailStatus.UnitNameMaxLength24;
        }
        else if (Suryo < 0)
        {
            return SaveSuperSetDetailStatus.InvalidSetOrderSuryo;
        }
        else if (UnitSBT < 0)
        {
            return SaveSuperSetDetailStatus.InvalidSetOrderUnitSBT;
        }
        else if (TermVal < 0)
        {
            return SaveSuperSetDetailStatus.InvalidSetOrderTermVal;
        }
        else if (KohatuKbn < 0)
        {
            return SaveSuperSetDetailStatus.InvalidSetOrderKohatuKbn;
        }
        else if (SyohoKbn < 0)
        {
            return SaveSuperSetDetailStatus.InvalidSetOrderSyohoKbn;
        }
        else if (SyohoLimitKbn < 0)
        {
            return SaveSuperSetDetailStatus.InvalidSetOrderSyohoLimitKbn;
        }
        else if (DrugKbn < 0)
        {
            return SaveSuperSetDetailStatus.InvalidSetOrderDrugKbn;
        }
        else if (YohoKbn < 0)
        {
            return SaveSuperSetDetailStatus.InvalidSetOrderYohoKbn;
        }
        else if (Kokuji1.Length > 1)
        {
            return SaveSuperSetDetailStatus.Kokuji1MaxLength1;
        }
        else if (Kokuji2.Length > 1)
        {
            return SaveSuperSetDetailStatus.Kokuji2MaxLength1;
        }
        else if (IsNodspRece < 0)
        {
            return SaveSuperSetDetailStatus.InvalidSetOrderIsNodspRece;
        }
        else if (IpnCd.Length > 12)
        {
            return SaveSuperSetDetailStatus.IpnCdMaxLength12;
        }
        else if (IpnName.Length > 120)
        {
            return SaveSuperSetDetailStatus.IpnNameMaxLength120;
        }
        else if (Bunkatu.Length > 10)
        {
            return SaveSuperSetDetailStatus.BunkatuMaxLength10;
        }
        else if (CmtName.Length > 240)
        {
            return SaveSuperSetDetailStatus.CmtNameMaxLength240;
        }
        else if (CmtOpt.Length > 38)
        {
            return SaveSuperSetDetailStatus.CmtOptMaxLength38;
        }
        else if (FontColor.Length > 8)
        {
            return SaveSuperSetDetailStatus.FontColorMaxLength8;
        }
        else if (CommentNewline < 0)
        {
            return SaveSuperSetDetailStatus.InvalidSetOrderCommentNewline;
        }
        return SaveSuperSetDetailStatus.ValidateOrderDetailSuccess;
    }
}
