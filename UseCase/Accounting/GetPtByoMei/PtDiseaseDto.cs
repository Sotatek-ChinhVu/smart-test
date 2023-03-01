using Helper.Common;

namespace UseCase.Accounting.GetPtByoMei
{
    public class PtDiseaseDto
    {
        public PtDiseaseDto(long ptId, string byomeiCd, long seqNo, int syubyoKbn, int sikkanKbn, string fullByomei, int startDate, string tenKiBinding, int tenkiDate)
        {
            PtId = ptId;
            ByomeiCd = byomeiCd;
            SeqNo = seqNo;
            SyubyoKbn = syubyoKbn;
            SikkanKbn = sikkanKbn;
            FullByomei = fullByomei;
            StartDate = CIUtil.SDateToShowSDate(startDate);
            TenKiBinding = tenKiBinding;
            TenkiDate = CIUtil.SDateToShowSDate(tenkiDate);
        }

        public long PtId { get; private set; }
        public string ByomeiCd { get; private set; }
        public long SeqNo { get; private set; }
        public int SyubyoKbn { get; private set; }
        public int SikkanKbn { get; private set; }
        public string FullByomei { get; private set; }
        public string StartDate { get; private set; }
        public string TenKiBinding { get; private set; }
        public string TenkiDate { get; private set; }
    }
}