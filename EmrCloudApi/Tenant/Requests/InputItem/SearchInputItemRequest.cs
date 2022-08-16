namespace EmrCloudApi.Tenant.Requests.InputItem
{
    public class SearchInputItemRequest
    {
        public string Keyword { get; set; } = string.Empty;

        public int KouiKbn { get; set; }

        public int SinDate { get; set; }

        public int StartIndex { get; set; }

        public int PageCount { get; set; }

        public bool IsSearchInline { get; set; }

        public string YJCd { get; set; } = string.Empty;

        public int HpId { get; set; }

        public double PointFrom { get; set; }

        public double PointTo { get; set; }

        public bool IsRosai { get; set; }

        public bool IsMirai { get; set; }

        public bool IsExpired { get; set; }
    }
}
