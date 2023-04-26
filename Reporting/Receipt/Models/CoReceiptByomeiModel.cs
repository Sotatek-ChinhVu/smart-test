namespace Reporting.Receipt.Models
{
    public class CoReceiptByomeiModel
    {
        private string _byomei;
        private string _startDate;
        private string _tenki;

        /// <summary>
        /// 病名
        /// </summary>
        public string Byomei
        {
            get { return _byomei ?? ""; }
            set { _byomei = value; }
        }

        /// <summary>
        /// 開始日（和暦）
        /// </summary>
        public string StartDate
        {
            get { return _startDate ?? ""; }
            set { _startDate = value; }
        }

        /// <summary>
        /// 転帰
        /// </summary>
        public string Tenki
        {
            get { return _tenki ?? ""; }
            set { _tenki = value; }
        }
    }
}
