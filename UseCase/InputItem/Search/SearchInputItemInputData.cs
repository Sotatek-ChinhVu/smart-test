using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.InputItem.Search
{
    public class SearchInputItemInputData : IInputData<SearchInputItemOutputData>
    {
        public SearchInputItemInputData(string keyword, int kouiKbn, int sinDate, int startIndex, int pageCount, int genericOrSameItem, string yJCd, int hpId, double pointFrom, double pointTo, bool isRosai, bool isMirai, bool isExpired)
        {
            Keyword = keyword;
            KouiKbn = kouiKbn;
            SinDate = sinDate;
            StartIndex = startIndex;
            PageCount = pageCount;
            GenericOrSameItem = genericOrSameItem;
            YJCd = yJCd;
            HpId = hpId;
            PointFrom = pointFrom;
            PointTo = pointTo;
            IsRosai = isRosai;
            IsMirai = isMirai;
            IsExpired = isExpired;
        }

        public string Keyword { get; private set; }

        public int KouiKbn { get; private set; }

        public int SinDate { get; private set; }

        public int StartIndex { get; private set; }

        public int PageCount { get; private set; }

        public int GenericOrSameItem { get; private set; }

        public string YJCd { get; private set; }

        public int HpId { get; private set; }

        public double PointFrom { get; private set; }

        public double PointTo { get; private set; }

        public bool IsRosai { get; private set; }

        public bool IsMirai { get; private set; }

        public bool IsExpired { get; private set; }
    }
}
