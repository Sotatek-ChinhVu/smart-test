namespace EmrCloudApi.Tenant.Requests.MedicalExamination
{
    public class OdrInfDetailItem
    {
        public int HpId { get; set; }
        public long RaiinNo { get; set; }
        public long RpNo { get; set; }
        public long RpEdaNo { get; set; }
        public int RowNo { get; set; }
        public long PtId { get; set; }
        public int SinDate { get; set; }
        public int SinKouiKbn { get; set; }
        public string ItemCd { get; set; } = string.Empty;
        public string ItemName { get; set; } = string.Empty;
        public double Suryo { get; set; }
        public string UnitName { get; set; } = string.Empty;
        public int UnitSbt { get; set; }
        public double TermVal { get; set; }
        public int KohatuKbn { get; set; }
        public int SyohoKbn { get; set; }
        public int SyohoLimitKbn { get; set; }
        public int DrugKbn { get; set; }
        public int YohoKbn { get; set; }
        public string Kokuji1 { get; set; } = string.Empty;
        public string Kokuji2 { get; set; } = string.Empty;
        public int IsNodspRece { get; set; }
        public string IpnCd { get; set; } = string.Empty;
        public string IpnName { get; set; } = string.Empty;
        public int JissiKbn { get; set; }
        public DateTime JissiDate { get; set; }
        public int JissiId { get; set; }
        public string JissiMachine { get; set; } = string.Empty;
        public string ReqCd { get; set; } = string.Empty;
        public string Bunkatu { get; set; } = string.Empty;
        public string CmtName { get; set; } = string.Empty;
        public string CmtOpt { get; set; } = string.Empty;
        public string FontColor { get; set; } = string.Empty;
        public int CommentNewline { get; set; }
    }
}
