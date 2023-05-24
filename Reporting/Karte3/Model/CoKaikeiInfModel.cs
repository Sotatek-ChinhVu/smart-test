namespace Reporting.Karte3.Model
{
    public class CoKaikeiInfModel
    {
        /// <summary>
        /// 会計情報
        /// </summary>
        public CoKaikeiInfModel(int hpId, long ptId, int sinDate, int hokenId, int tensu, int ptFutan, int jihiFutan, int outtax, int totalPtFutan)
        {
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
            HokenId = hokenId;
            Tensu = tensu;
            PtFutan = ptFutan;
            JihiFutan = jihiFutan;
            Outtax = outtax;
            TotalPtFutan = totalPtFutan;
        }

        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        public int HpId { get; set; }
        /// <summary>
        /// 患者ID
        /// </summary>
        public long PtId { get; set; }
        /// <summary>
        /// 診療日
        /// </summary>
        public int SinDate { get; set; }
        /// <summary>
        /// 保険ID
        /// </summary>
        public int HokenId { get; set; }
        /// <summary>
        /// 点数
        /// </summary>
        public int Tensu { get; set; }
        /// <summary>
        /// 患者負担
        /// </summary>
        public int PtFutan { get; set; }
        /// <summary>
        /// 自費負担
        /// </summary>
        public int JihiFutan { get; set; }
        /// <summary>
        /// 外税
        /// </summary>
        public int Outtax { get; set; }
        /// <summary>
        /// 合計患者負担
        /// </summary>
        public int TotalPtFutan { get; set; }
    }
}
