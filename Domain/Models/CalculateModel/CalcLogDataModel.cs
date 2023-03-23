namespace Domain.Models.CalculateModel
{
    public class CalcLogDataModel
    {
        public int HpId { get; set; }
        public int PtId { get; set; }
        public int SinDate { get; set; }
        public int RaiinNo { get; set; }
        public int SeqNo { get; set; }
        public int LogSbt { get; set; }
        public string Text { get; set; } = string.Empty;
        public int HokenId { get; set; }
        public string ItemCd { get; set; } = string.Empty;
        public string DelItemCd { get; set; } = string.Empty;
        public int DelSbt { get; set; }
        public int IsWarning { get; set; }
        public int TermCnt { get; set; }
        public int TermSbt { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateId { get; set; }
        public string CreateMachine { get; set; } = string.Empty;
        public DateTime UpdateDate { get; set; }
        public int UpdateId { get; set; }
        public string UpdateMachine { get; set; } = string.Empty;
    }

    public class CalcLogList
    {
        public CalcLogDataModel CalcLog { get; set; } = new();
        public int HpId { get; set; }
        public int PtId { get; set; }
        public int SinDate { get; set; }
        public int RaiinNo { get; set; }
        public int SeqNo { get; set; }
        public int LogSbt { get; set; }
        public string Text { get; set; } = string.Empty;
        public int HokenId { get; set; }
        public string ItemCd { get; set; } = string.Empty;
        public string DelItemCd { get; set; } = string.Empty;
        public int DelSbt { get; set; }
        public int IsWarning { get; set; }
        public int TermCnt { get; set; }
        public int TermSbt { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateId { get; set; }
        public string CreateMachine { get; set; } = string.Empty;
        public DateTime UpdateDate { get; set; }
        public int UpdateId { get; set; }
        public string UpdateMachine { get; set; } = string.Empty;
    }
}
