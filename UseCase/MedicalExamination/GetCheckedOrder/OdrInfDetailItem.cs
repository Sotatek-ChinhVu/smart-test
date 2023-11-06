namespace UseCase.MedicalExamination.GetCheckedOrder
{
    public class OdrInfDetailItem
    {
        public OdrInfDetailItem(int hpId, long ptId, int sinDate, long raiinNo, long rpNo, long rpEdaNo, int rowNo, int sinKouiKbn, string itemCd, double suryo, string unitName, double termVal, int syohoKbn, int drugKbn, int yohoKbn, string kokuji1, string kokuji2, int isNodspRece, string ipnCd, string ipnName, string cmtOpt, string itemName, bool isDummy)
        {
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
            RaiinNo = raiinNo;
            RpNo = rpNo;
            RpEdaNo = rpEdaNo;
            RowNo = rowNo;
            SinKouiKbn = sinKouiKbn;
            ItemCd = itemCd;
            Suryo = suryo;
            UnitName = unitName;
            TermVal = termVal;
            SyohoKbn = syohoKbn;
            DrugKbn = drugKbn;
            YohoKbn = yohoKbn;
            Kokuji1 = kokuji1;
            Kokuji2 = kokuji2;
            IsNodspRece = isNodspRece;
            IpnCd = ipnCd;
            IpnName = ipnName;
            CmtOpt = cmtOpt;
            ItemName = itemName;
            IsDummy = isDummy;
        }

        public int HpId { get; set; }

        public long PtId { get; set; }

        public int SinDate { get; set; }

        public long RaiinNo { get; set; }

        public long RpNo { get; set; }

        public long RpEdaNo { get; set; }

        public int RowNo { get; set; }

        public int SinKouiKbn { get; set; }

        public string ItemCd { get; set; } = string.Empty;

        public double Suryo { get; set; }

        public string UnitName { get; set; } = string.Empty;

        public double TermVal { get; set; }

        public int SyohoKbn { get; set; }

        public int DrugKbn { get; set; }

        public int YohoKbn { get; set; }

        public string Kokuji1 { get; set; } = string.Empty;

        public string Kokuji2 { get; set; } = string.Empty;

        public int IsNodspRece { get; set; }

        public string IpnCd { get; set; } = string.Empty;

        public string IpnName { get; set; } = string.Empty;

        public string CmtOpt { get; set; } = string.Empty;

        public string ItemName { get; set; } = string.Empty;

        public bool IsDummy { get; set; } = false;
    }
}
