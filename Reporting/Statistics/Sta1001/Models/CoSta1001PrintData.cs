using Helper.Common;
using Reporting.Statistics.Enums;

namespace Reporting.Statistics.Sta1001.Models
{
    public class CoSta1001PrintData
    {
        public CoSta1001PrintData(RowType rowType = RowType.Data)
        {
            RowType = rowType;
        }

        public RowType RowType { get; set; }

        public string TotalCaption { get; set; }

        public string TotalCount { get; set; }

        public string TotalPtCount { get; set; }

        public long RaiinNo { get; set; }

        public long OyaRaiinNo { get; set; }

        public string Seq { get; set; }

        public string PtNum { get; set; }

        public int SinDate { get; set; }

        public string SinDateFmt
        {
            get => CIUtil.SDateToShowSDate(SinDate);
        }

        public string PtName { get; set; }

        public string PtKanaName { get; set; }

        public string HokenSbt { get; set; }

        public string Syosaisin { get; set; }

        public string Tensu { get; set; }

        public string NewTensu { get; set; }

        public string PtFutan { get; set; }

        public string PtFutanJihiRece { get; set; }

        public string PtFutanElse { get; set; }

        public string JihiFutan { get; set; }

        public string JihiTax { get; set; }

        public string AdjustFutan { get; set; }

        public string SeikyuGaku { get; set; }

        public string NewSeikyuGaku { get; set; }

        public string NyukinKbn { get; set; }

        public string PaySname { get; set; }

        public int PayCd { get; set; }

        public string MenjyoGaku { get; set; }

        public string NyukinGaku { get; set; }

        public int NyukinDate { get; set; }

        public string NyukinDateFmt
        {
            get => CIUtil.SDateToShowSDate(NyukinDate);
        }

        public string PreNyukinGaku { get; set; }

        public string MisyuGaku { get; set; }

        public int UketukeSbt { get; set; }

        public string UketukeSbtName { get; set; }

        public int NyukinUserId { get; set; }

        public string NyukinUserSname { get; set; }

        public string NyukinTime { get; set; }

        public int KaId { get; set; }

        public string KaSname { get; set; }

        public int TantoId { get; set; }

        public string TantoSname { get; set; }

        public int UketukeId { get; set; }

        public string UketukeSname { get; set; }

        public string UketukeTime { get; set; }

        public string KaikeiTime { get; set; }

        public string RaiinCmt { get; set; }

        public string NyukinCmt { get; set; }

        public List<string> JihiSbtFutans { get; set; }
    }
}
