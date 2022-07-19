namespace Domain.Models.OrdInfDetails
{
    public class OrdInfDetailModel
    {
        public int HpId { get; set; }
        public long RaiinNo { get; set; }
        public long RpNo { get; set; }
        public long RpEdaNo { get; set; }
        public int RowNo { get; set; }
        public long PtId { get; set; }
        public int SinDate { get; set; }
        public int SinKouiKbn { get; set; }
        public string? ItemCd { get; set; }
        public string? ItemName { get; set; }
        public double Suryo { get; set; }
        public string? UnitName { get; set; }
        public int UnitSbt { get; set; }
        public double TermVal { get; set; }
        public int KohatuKbn { get; set; }
        public int SyohoKbn { get; set; }
        public int SyohoLimitKbn { get; set; }
        public int DrugKbn { get; set; }
        public int YohoKbn { get; set; }
        public string? Kokuji1 { get; set; }
        public string? Kokuji2 { get; set; }
        public int IsNodspRece { get; set; }
        public string? IpnCd { get; set; }
        public string? IpnName { get; set; }
        public int JissiKbn { get; set; }
        public DateTime? JissiDate { get; set; }
        public int JissiId { get; set; }
        public string? JissiMachine { get; set; }
        public string? ReqCd { get; set; }
        public string? Bunkatu { get; set; }
        public string? CmtName { get; set; }
        public string? CmtOpt { get; set; }
        public string? FontColor { get; set; }
        public int CommentNewline { get; set; }

        public OrdInfDetailModel(int hpId, long raiinNo, long rpNo, long rpEdaNo, int rowNo, long ptId, int sinDate, int sinKouiKbn, string? itemCd, string? itemName, double suryo, string? unitName, int unitSbt, double termVal, int kohatuKbn, int syohoKbn, int syohoLimitKbn, int drugKbn, int yohoKbn, string? kokuji1, string? kokuji2, int isNodspRece, string? ipnCd, string? ipnName, int jissiKbn, DateTime? jissiDate, int jissiId, string? jissiMachine, string? reqCd, string? bunkatu, string? cmtName, string? cmtOpt, string? fontColor, int commentNewline)
        {
            HpId = hpId;
            RaiinNo = raiinNo;
            RpNo = rpNo;
            RpEdaNo = rpEdaNo;
            RowNo = rowNo;
            PtId = ptId;
            SinDate = sinDate;
            SyohoLimitKbn = syohoLimitKbn;
            SinKouiKbn = sinKouiKbn;
            ItemCd = itemCd;
            ItemName = itemName;
            Suryo = suryo;
            UnitName = unitName;
            UnitSbt = unitSbt;
            TermVal = termVal;
            KohatuKbn = kohatuKbn;
            SyohoKbn = syohoKbn;
            SyohoLimitKbn = syohoKbn;
            DrugKbn = drugKbn;
            YohoKbn = yohoKbn;
            Kokuji1 = kokuji1;
            Kokuji2 = kokuji2;
            IsNodspRece = isNodspRece;
            IpnCd = ipnCd;
            IpnName = ipnName;
            JissiKbn = jissiKbn;
            JissiDate = jissiDate;
            JissiId = jissiId;
            JissiMachine = jissiMachine;
            ReqCd = reqCd;
            Bunkatu = bunkatu;
            CmtName = cmtName;
            CmtOpt = ipnCd;
            CmtOpt = cmtOpt;
            FontColor = fontColor;
            CommentNewline = commentNewline;
        }
    }
}
