namespace EmrCloudApi.Requests.MstItem
{
    public class DiseaseNameMstSearchRequest
    {
        public string Keyword { get; set; } = string.Empty;
        public bool ChkByoKbn0 { get; set; } = false;
        public bool ChkByoKbn1 { get; set; } = false;
        public bool ChkSaiKbn { get; set; } = false;
        public bool ChkMiSaiKbn { get; set; } = false;
        public bool ChkSidoKbn { get; set; } = false;
        public bool ChkToku { get; set; } = false;
        public bool ChkHiToku1 { get; set; } = false;
        public bool ChkHiToku2 { get; set; } = false;
        public bool ChkTenkan { get; set; } = false;
        public bool ChkTokuTenkan { get; set; } = false;
        public bool ChkNanbyo { get; set; } = false;
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 0;
        public bool IsCheckPage { get; set; } = false;
    }
}
