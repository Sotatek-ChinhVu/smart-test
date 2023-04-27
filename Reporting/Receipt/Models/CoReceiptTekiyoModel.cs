namespace Reporting.Receipt.Models
{
    public class CoReceiptTekiyoModel
    {
        private string _sinId;
        private string _mark;
        private string _tekiyo;

        public CoReceiptTekiyoModel()
        {
            DayCount = new List<int>
            {
                0,0,0,0,0,0,0,0,0,0,
                0,0,0,0,0,0,0,0,0,0,
                0,0,0,0,0,0,0,0,0,0,
                0
            };
        }
        /// <summary>
        /// 診療区分
        /// </summary>
        public string SinId
        {
            get { return _sinId ?? ""; }
            set { _sinId = value; }
        }
        /// <summary>
        /// マーク
        /// </summary>
        public string Mark
        {
            get { return _mark ?? ""; }
            set { _mark = value; }
        }
        /// <summary>
        /// 摘要
        /// </summary>
        public string Tekiyo
        {
            get { return _tekiyo ?? ""; }
            set { _tekiyo = value; }
        }

        public List<int> DayCount { get; set; }
    }
}
