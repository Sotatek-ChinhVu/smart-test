namespace EmrCloudApi.Tenant.Requests.MedicalExamination
{
    public class ByomeiMstItem
    {
        public string ByomeiCd { get; set; } = string.Empty;

        public string ByomeiType { get; set; } = string.Empty;

        public string Sbyomei { get; set; } = string.Empty;

        public string KanaName1 { get; set; } = string.Empty;

        public string Sikkan { get; set; } = string.Empty;

        public string NanByo { get; set; } = string.Empty;

        public string Icd10 { get; set; } = string.Empty;

        public string Icd102013 { get; set; } = string.Empty;

        public int IsAdopted { get; set; }
    }
}
