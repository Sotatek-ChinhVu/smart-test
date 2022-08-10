using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.PtKioReki
{
    public class PtKioRekiModel
    {
        public PtKioRekiModel(int hpId, long ptId, int seqNo, int sortNo, string byomeiCd, string byotaiCd, string byomei, int startDate, string cmt, int isDeleted)
        {
            HpId = hpId;
            PtId = ptId;
            SeqNo = seqNo;
            SortNo = sortNo;
            ByomeiCd = byomeiCd;
            ByotaiCd = byotaiCd;
            Byomei = byomei;
            StartDate = startDate;
            Cmt = cmt;
            IsDeleted = isDeleted;
        }

        public int HpId { get; private set; }
        public long PtId { get; private set; }
        public int SeqNo { get; private set; }
        public int SortNo { get; private set; }
        public string ByomeiCd { get; private set; }
        public string ByotaiCd { get; private set; }
        public string Byomei { get; private set; }
        public int StartDate { get; private set; }
        public string Cmt { get; private set; }
        public int IsDeleted { get; private set; }
    }
}
