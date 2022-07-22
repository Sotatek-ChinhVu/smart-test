using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.KarteKbn
{
    public class KarteKbnMst
    {
        public KarteKbnMst(int hpId, int karteKbn, string? kbnName, string? kbnShortName, int canImg, int sortNo, int isDeleted)
        {
            HpId = hpId;
            KarteKbn = karteKbn;
            KbnName = kbnName;
            KbnShortName = kbnShortName;
            CanImg = canImg;
            SortNo = sortNo;
            IsDeleted = isDeleted;
        }

        public int HpId { get; private set; }
        public int KarteKbn { get; private set; }
        public string? KbnName { get; private set; }
        public string? KbnShortName { get; private set; }
        public int CanImg { get; private set; }
        public int SortNo { get; private set; }
        public int IsDeleted { get; private set; }
    }
}
