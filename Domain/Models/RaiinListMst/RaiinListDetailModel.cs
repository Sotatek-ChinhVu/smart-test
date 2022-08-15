using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.RaiinListMst
{
    public class RaiinListDetailModel
    {
        public int GrpId { get; private set; }

        public int KbnCd { get; private set; }

        public int SortNo { get; private set; }

        public string KbnName { get; private set; }

        public string ColorCd { get; private set; }

        public int IsDeleted { get; private set; }

        public RaiinListDetailModel(int grpId, int kbnCd, int sortNo, string kbnName, string colorCd, int isDeleted)
        {
            GrpId = grpId;
            KbnCd = kbnCd;
            SortNo = sortNo;
            KbnName = kbnName;
            ColorCd = colorCd;
            IsDeleted = isDeleted;
        }
    }
}
