using Helper.Common;

namespace UseCase.Accounting.GetPtByoMei
{
    public class PtDiseaseDto
    {
        public PtDiseaseDto(string fullByomei, int startDate, string tenKiBinding, int tenkiDate)
        {
            FullByomei = fullByomei;
            StartDate = CIUtil.SDateToShowSDate(startDate);
            TenKiBinding = tenKiBinding;
            TenkiDate = CIUtil.SDateToShowSDate(tenkiDate);
        }

        public string FullByomei { get; private set; }
        public string StartDate { get; private set; }
        public string TenKiBinding { get; private set; }
        public string TenkiDate { get; private set; }
    }
}
