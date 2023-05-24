namespace Reporting.Karte3.Model
{
    class CoSinKouiModel
    {
        public CoSinKouiModel(long ptId, int sinDate, int rpNo, int seqNo, int sinId, string syukeiSaki, double ten, int count, int entenKbn, int santeiKbn, string cdKbn, string cdno, int hokenId)
        {
            PtId = ptId;
            SinDate = sinDate;
            RpNo = rpNo;
            SeqNo = seqNo;
            SinId = sinId;
            SyukeiSaki = syukeiSaki;
            Ten = ten;
            Count = count;
            EntenKbn = entenKbn;
            SanteiKbn = santeiKbn;
            CdKbn = cdKbn;
            CdNo = cdno;
            HokenId = hokenId;
        }

        /// <summary>
        /// 患者ID
        /// </summary>
        public long PtId { get; set; }
        /// <summary>
        /// 診療日
        /// </summary>
        public int SinDate { get; set; }
        /// <summary>
        /// RpNo
        /// </summary>
        public int RpNo { get; set; }
        /// <summary>
        /// SeqNo
        /// </summary>
        public int SeqNo { get; set; }
        /// <summary>
        /// 診療区分
        /// </summary>
        public int SinId { get; set; }
        /// <summary>
        /// 集計先
        /// </summary>
        public string SyukeiSaki { get; set; }
        /// <summary>
        /// 点数
        /// </summary>
        public double Ten { get; set; }
        /// <summary>
        /// 回数
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 点数×回数
        /// </summary>
        public double TotalTen
        {
            get { return Ten * Count; }
        }
        public double TotalTenDsp
        {
            get { return TotalTen * (EntenKbn == 0 ? 1 : 0.1); }
        }
        /// <summary>
        /// 円点区分
        /// </summary>
        public int EntenKbn { get; set; }
        /// <summary>
        /// 算定区分
        /// </summary>
        public int SanteiKbn { get; set; }
        public int SanteiKbnDsp
        {
            get
            {
                int ret = SanteiKbn;

                if (new string[] { "SZ", "SK" }.Contains(CdKbn))
                {
                    ret = 2;    //自費算定扱い
                }

                return ret;
            }
        }
        /// <summary>
        /// コード区分
        /// </summary>
        public string CdKbn { get; set; }
        public string CdNo { get; set; }
        /// <summary>
        /// 保険ID
        /// </summary>
        public int HokenId { get; set; }
    }
}
