namespace EmrCloudApi.Tenant.Requests.InputItem
{
    public class SearchInputItemRequest
    {
        public string Keyword { get; set; } = string.Empty;

        public int KouiKbn { get; set; }

        public int SinDate { get; set; }

        public int StartIndex { get; set; }

        public int PageCount { get; set; }

        public int GenericOrSameItem { get; set; } = 0;

        public string YJCd { get; set; } = string.Empty;

        public int HpId { get; set; }

        public double PointFrom { get; set; } = 0; 

        public double PointTo { get; set; } = 0;

        public bool IsRosai { get; set; } = false;

        public bool IsMirai { get; set; } = false;

        public bool IsExpired { get; set; } = false;
    }
}
