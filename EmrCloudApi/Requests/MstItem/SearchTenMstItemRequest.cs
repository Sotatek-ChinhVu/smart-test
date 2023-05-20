using Helper.Enum;

namespace EmrCloudApi.Requests.MstItem
{
    public class SearchTenMstItemRequest
    {
        public int HpId { get; private set; }
        public int PageIndex { get; private set; }
        public int PageCount { get; private set; }

        public string Keyword { get; private set; } = string.Empty;
        public double PointFrom { get; private set; } = -1;
        public double PointTo { get; private set; } = -1;
        public int KouiKbn { get; private set; }
        public int OriKouiKbn { get; private set; }
        public List<int> KouiKbns { get; private set; } = new();
        public bool IncludeRosai { get; private set; }
        public bool IncludeMisai { get; private set; }
        public int STDDate { get; private set; }
        public string ItemCodeStartWith { get; private set; } = string.Empty;
        public bool IsIncludeUsage { get; private set; } = true;
        public bool OnlyUsage { get; private set; } = false;
        public string YJCode { get; private set; } = string.Empty;
        public bool IsMasterSearch { get; private set; } = false;
        public bool IsExpiredSearchIfNoData { get; private set; } = false;
        public bool IsAllowSearchDeletedItem { get; private set; } = false;
        public bool IsExpired { get; private set; } = false;
        public bool IsDeleted { get; private set; } = false;
        public List<int> DrugKbns { get; private set; } = new();
        public bool IsSearchSanteiItem { get; private set; } = false;
        public bool IsSearchKenSaItem { get; private set; } = false;
        public List<ItemTypeEnums> ItemFilter { get; private set; } = new();
        public bool IsSearch831SuffixOnly { get; private set; } = false;
    }
}
