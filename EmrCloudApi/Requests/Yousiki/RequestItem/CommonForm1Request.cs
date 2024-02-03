using Helper.Enum;

namespace EmrCloudApi.Requests.Yousiki.RequestItem
{
    public class CommonForm1Request
    {
        public int PayLoadValueSelect { get; set; } = 1;

        public Yousiki1InfDetailRequest ValueSelect { get; set; } = new();

        public int PayLoadInjuryName { get; set; } = 9;

        public Yousiki1InfDetailRequest InjuryNameLast { get; set; } = new();

        public int PayLoadICD10Code { get; set; } = 2;

        public Yousiki1InfDetailRequest Icd10 { get; set; } = new();

        public int PayLoadInjuryNameCode { get; set; } = 3;

        public Yousiki1InfDetailRequest InjuryNameCode { get; set; } = new();

        public int PayLoadModifierCode { get; set; } = 4;

        public Yousiki1InfDetailRequest ModifierCode { get; set; } = new();

        public int SortNo { get; set; }

        public long PtId { get; set; }

        public int SinYm { get; set; }

        public int DataType { get; set; }

        public int SeqNo { get; set; }

        public string CodeNo { get; set; } = string.Empty;

        public bool IsDeleted { get; set; }

        public int DateOfHospitalizationPayLoad { get; set; }

        public Yousiki1InfDetailRequest DateOfHospitalization { get; set; } = new();

        public int DischargeDatePayLoad { get; set; }

        public Yousiki1InfDetailRequest DischargeDate { get; set; } = new();

        public int DestinationPayLoad { get; set; }

        public Yousiki1InfDetailRequest Destination { get; set; } = new();

        public bool IsEnableICD10Code { get; set; }

        public int HouseCallDatePayLoad { get; set; }

        public Yousiki1InfDetailRequest HouseCallDate { get; set; } = new();

        public Yousiki1InfDetailRequest MedicalInstitution { get; set; } = new();

        public int MedicalInstitutionPayLoad { get; set; }

        public int StartDatePayLoad { get; set; }

        public Yousiki1InfDetailRequest StartDate { get; set; } = new();

        public int OnsetDatePayLoad { get; set; }

        public Yousiki1InfDetailRequest OnsetDate { get; set; } = new();

        public int MaximumNumberDatePayLoad { get; set; }

        public Yousiki1InfDetailRequest MaximumNumberDate { get; set; } = new();

        public ByomeiListType GridType { get; set; } = ByomeiListType.None;

        public string ByomeiCd { get; set; } = string.Empty;

        public string Byomei { get; set; } = string.Empty;

        public string FullByomei {  get; set; } = string.Empty;

        public List<PrefixSuffixRequest> PrefixSuffixList { get; set; } = new();
    }
}
