namespace Reporting.Statistics.Sta1001.Models
{
    public class CoSta1001PrintConf
    {
        public CoSta1001PrintConf(int menuId)
        {
            MenuId = menuId;
            StartNyukinTime = -1;
            EndNyukinTime = -1;
        }

        public int MenuId { get; }

        public string FormFileName { get; set; }

        public string ReportName { get; set; }

        public bool IsTester { get; set; }

        public int StartNyukinDate { get; set; }

        public int EndNyukinDate { get; set; }

        public int StartNyukinTime { get; set; }

        public int EndNyukinTime { get; set; }

        public bool IsExcludeUnpaid { get; set; }

        public int PageBreak1 { get; set; }

        public int PageBreak2 { get; set; }

        public int PageBreak3 { get; set; }

        public int SortOrder1 { get; set; }

        public int SortOpt1 { get; set; }

        public int SortOrder2 { get; set; }

        public int SortOpt2 { get; set; }

        public int SortOrder3 { get; set; }

        public int SortOpt3 { get; set; }

        public List<int> UketukeSbtIds { get; set; }

        public List<int> KaIds { get; set; }

        public List<int> TantoIds { get; set; }

        public List<int> PaymentMethodCds { get; set; }
    }
}
