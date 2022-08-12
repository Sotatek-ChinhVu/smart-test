using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.InputItem
{
    public class SearchItemCondition
    {
        public SearchItemCondition(bool isSearchWithItem, string keyword, int sinDate, int kouiKbn, List<int> kouiKbns, double pointFrom, double pointTo, bool includeRosai, bool includeMisai,  string itemCodeStartWith, List<int> drugKbns, string yJCode)
        {
            Keyword = keyword;
            PointFrom = pointFrom;
            PointTo = pointTo;
            KouiKbn = kouiKbn;
            KouiKbns = kouiKbns;
            IncludeRosai = includeRosai;
            IncludeMisai = includeMisai;
            SinDate = sinDate;
            ItemCodeStartWith = itemCodeStartWith;
            DrugKbns = drugKbns;
            YJCode = yJCode;
            IsSearchWithItem = isSearchWithItem;
        }

        public string Keyword { get; set; }

        public double PointFrom { get; set; }

        public double PointTo { get; set; }

        public int KouiKbn { get; set; }

        public List<int> KouiKbns { get; set; }

        public bool IncludeRosai { get; set; }

        public bool IncludeMisai { get; set; }

        public int SinDate { get; set; }

        public string ItemCodeStartWith { get; set; }

        public List<int> DrugKbns { get; set; }

        public string YJCode { get; set; }

        public bool IsSearchWithItem { get; set; }

        public bool IsIncludeUsage { get; set; } = true;

        public bool OnlyUsage { get; set; } = false;

        public bool IsMasterSearch { get; set; } = false;

        public bool IsExpiredSearchIfNoData { get; set; } = false;

        public bool IsExpired { get; set; } = false;

        public bool IsSearchSanteiItem { get; set; } = false;

        public bool IsSearchKenSaItem { get; set; } = false;

        public bool IsSearch831SuffixOnly { get; set; } = false;
    }
}
