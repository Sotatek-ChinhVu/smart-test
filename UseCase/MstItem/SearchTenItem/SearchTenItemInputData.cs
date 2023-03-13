using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.SearchTenItem
{
    public class SearchTenItemInputData : IInputData<SearchTenItemOutputData>
    {
        public SearchTenItemInputData(string keyword, int kouiKbn, int sinDate, int pageIndex, int pageCount, int genericOrSameItem, string yJCd, int hpId, double pointFrom, double pointTo, bool isRosai, bool isMirai, bool isExpired, string itemCodeStartWith, bool isMasterSearch, bool isSearch831SuffixOnly, bool isSearchSanteiItem, byte searchFollowUsage)
        {
            Keyword = keyword;
            KouiKbn = kouiKbn;
            SinDate = sinDate;
            PageIndex = pageIndex;
            PageCount = pageCount;
            GenericOrSameItem = genericOrSameItem;
            YJCd = yJCd;
            HpId = hpId;
            PointFrom = pointFrom;
            PointTo = pointTo;
            IsRosai = isRosai;
            IsMirai = isMirai;
            IsExpired = isExpired;
            ItemCodeStartWith = itemCodeStartWith;
            IsMasterSearch = isMasterSearch;
            IsSearch831SuffixOnly = isSearch831SuffixOnly;
            IsSearchSanteiItem = isSearchSanteiItem;
            SearchFollowUsage = searchFollowUsage;
        }

        public string Keyword { get; private set; }

        public int KouiKbn { get; private set; }

        public int SinDate { get; private set; }

        public int PageIndex { get; private set; }

        public int PageCount { get; private set; }

        public int GenericOrSameItem { get; private set; }

        public string YJCd { get; private set; }

        public int HpId { get; private set; }

        public double PointFrom { get; private set; }

        public double PointTo { get; private set; }

        public bool IsRosai { get; private set; }

        public bool IsMirai { get; private set; }

        public bool IsExpired { get; private set; }

        public string ItemCodeStartWith { get; private set; }

        public bool IsMasterSearch { get; private set; }

        public bool IsSearch831SuffixOnly { get; private set; }

        public bool IsSearchSanteiItem { get; private set; }

        public byte SearchFollowUsage { get; private set; }
    }
}
