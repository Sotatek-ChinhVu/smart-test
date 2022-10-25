using Helper.Common;
using Helper.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.MstItem
{
    public class ItemCmtModel
    {
        public ItemCmtModel(string itemCd, int karteKbn, string karteColumn, bool isVisible, string comment, int sortNo)
        {
            ItemCd = itemCd;
            KarteKbn = karteKbn;
            KarteColumn = karteColumn;
            IsVisible = isVisible;
            Comment = comment;
            SortNo = sortNo;
        }

        public string ItemCd { get; private set; }

        public int KarteKbn { get; private set; }

        public string KarteColumn { get; private set; }

        public bool IsVisible { get; private set; }

        public string Comment { get; private set; }

        public int SortNo { get; private set; }

        public bool IsNotDefault => !CheckDefaultValue();

        public bool CheckDefaultValue()
        {
            return KarteKbn == 0;
        }
    }
}
