using Domain.Models.Diseases;

namespace UseCase.MedicalExamination.GetCheckDisease
{
    public class PtDiseaseItem
    {
        public PtDiseaseItem(PtDiseaseModel ptDiseaseModel)
        {
            IsFreeWord = ptDiseaseModel.IsFreeWord;
            IsTenki = ptDiseaseModel.IsTenki;
            IsContinous = ptDiseaseModel.IsContinous;
            IsInMonth = ptDiseaseModel.IsInMonth;
            HokenPid = ptDiseaseModel.HokenPid;
            Icd10 = ptDiseaseModel.Icd10;
            Icd102013 = ptDiseaseModel.Icd102013;
            Icd1012013 = ptDiseaseModel.Icd1012013;
            Icd1022013 = ptDiseaseModel.Icd1022013;
            HpId = ptDiseaseModel.HpId;
            PtId = ptDiseaseModel.PtId;
            SeqNo = ptDiseaseModel.SeqNo;
            ByomeiCd = ptDiseaseModel.ByomeiCd;
            SortNo = ptDiseaseModel.SortNo;
            PrefixSuffixList = ptDiseaseModel.PrefixSuffixList;
            Byomei = ptDiseaseModel.Byomei;
            IsSuspect = ptDiseaseModel.IsSuspect;
            StartDate = ptDiseaseModel.StartDate;
            TenkiKbn = ptDiseaseModel.TenkiKbn;
            TenkiDate = ptDiseaseModel.TenkiDate;
            SyubyoKbn = ptDiseaseModel.SyubyoKbn;
            SikkanKbn = ptDiseaseModel.SikkanKbn;
            NanbyoCd = ptDiseaseModel.NanbyoCd;
            IsNodspRece = ptDiseaseModel.IsNodspRece;
            IsNodspKarte = ptDiseaseModel.IsNodspKarte;
            IsDeleted = ptDiseaseModel.IsDeleted;
            Id = ptDiseaseModel.Id;
            IsImportant = ptDiseaseModel.IsImportant;
            SinDate = ptDiseaseModel.SinDate;
            HosokuCmt = ptDiseaseModel.HosokuCmt;
        }

        public bool IsFreeWord { get; private set; }

        public bool IsTenki { get; private set; }

        public bool IsContinous { get; private set; }

        public bool IsInMonth { get; private set; }

        public int HokenPid { get; private set; }

        public string Icd10 { get; private set; }

        public string Icd102013 { get; private set; }

        public string Icd1012013 { get; private set; }

        public string Icd1022013 { get; private set; }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public long SeqNo { get; private set; }

        public string ByomeiCd { get; private set; }

        public int SortNo { get; private set; }

        public List<PrefixSuffixModel> PrefixSuffixList { get; private set; }

        public string Byomei { get; private set; }

        public int IsSuspect { get; private set; }

        public int StartDate { get; private set; }

        public int TenkiKbn { get; private set; }

        public int TenkiDate { get; private set; }

        public int SyubyoKbn { get; private set; }

        public int SikkanKbn { get; private set; }

        public int NanbyoCd { get; private set; }

        public int IsNodspRece { get; private set; }

        public int IsNodspKarte { get; private set; }

        public int IsDeleted { get; private set; }

        public long Id { get; private set; }

        public int IsImportant { get; private set; }

        public int SinDate { get; private set; }

        public string HosokuCmt { get; private set; }
    }
}
