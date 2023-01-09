namespace UseCase.MedicalExamination.GetCheckedOrder
{
    public class DiseaseItem
    {
        public DiseaseItem(int sikkanKbn, int hokenPid, int startDate, int tenkiKbn, int tenkiDate, int syubyoKbn)
        {
            SikkanKbn = sikkanKbn;
            HokenPid = hokenPid;
            StartDate = startDate;
            TenkiKbn = tenkiKbn;
            TenkiDate = tenkiDate;
            SyubyoKbn = syubyoKbn;
        }

        public int SikkanKbn { get; private set; }

        public int HokenPid { get; private set; }

        public int StartDate { get; private set; }

        public int TenkiKbn { get; private set; }

        public int TenkiDate { get; private set; }

        public int SyubyoKbn { get; private set; }
    }
}
