namespace UseCase.MedicalExamination.GetCheckDisease
{
    public class CheckedDiseaseItem
    {
        public CheckedDiseaseItem(int sikkanCd, int nanByoCd, string byomei, int odrItemNo, PtDiseaseItem ptDisease, ByomeiItem byomeiMst, bool isAdopted)
        {
            SikkanCd = sikkanCd;
            NanByoCd = nanByoCd;
            Byomei = byomei;
            OdrItemNo = odrItemNo;
            PtDisease = ptDisease;
            ByomeiMst = byomeiMst;
            IsAdopted = isAdopted;
        }

        public int SikkanCd { get; private set; }

        public int NanByoCd { get; private set; }

        public string Byomei { get; private set; }

        public int OdrItemNo { get; private set; }

        public PtDiseaseItem PtDisease { get; private set; }

        public ByomeiItem ByomeiMst { get; private set; }

        public bool IsAdopted { get; private set; }
    }
}
