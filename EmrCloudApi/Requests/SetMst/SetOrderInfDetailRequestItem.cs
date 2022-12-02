namespace EmrCloudApi.Requests.SetMst;

public class SetOrderInfDetailRequestItem
{
    public int SinKouiKbn { get; set; }

    public string ItemCd { get; set; } = string.Empty;

    public string ItemName { get; set; } = string.Empty;

    public double Suryo { get; set; } = 0;

    public string UnitName { get; set; } = string.Empty;

    public int UnitSBT { get; set; } = 0;

    public double TermVal { get; set; } = 0;

    public int KohatuKbn { get; set; } = 0;

    public int SyohoKbn { get; set; } = 0;

    public int SyohoLimitKbn { get; set; } = 0;

    public int DrugKbn { get; set; } = 0;

    public int YohoKbn { get; set; } = 0;

    public string Kokuji1 { get; set; } = string.Empty;

    public string Kokuji2 { get; set; } = string.Empty;

    public int IsNodspRece { get; set; } = 0;

    public string IpnCd { get; set; } = string.Empty;

    public string IpnName { get; set; } = string.Empty;

    public string Bunkatu { get; set; } = string.Empty;

    public string CmtName { get; set; } = string.Empty;

    public string CmtOpt { get; set; } = string.Empty;

    public string FontColor { get; set; } = string.Empty;

    public int CommentNewline { get; set; } = 0;
}
