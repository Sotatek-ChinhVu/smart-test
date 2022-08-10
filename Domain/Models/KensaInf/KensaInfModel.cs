using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.KensaInf
{
    public class KensaInfModel
    {
        public KensaInfModel(int hpId, long ptId, long iraiCd, int iraiDate, long raiinNo, int inoutKbn, int status, int tosekiKbn, int sikyuKbn, int resultCheck, string centerCd, string nyubi, string yoketu, string bilirubin, int isDeleted)
        {
            HpId = hpId;
            PtId = ptId;
            IraiCd = iraiCd;
            IraiDate = iraiDate;
            RaiinNo = raiinNo;
            InoutKbn = inoutKbn;
            Status = status;
            TosekiKbn = tosekiKbn;
            SikyuKbn = sikyuKbn;
            ResultCheck = resultCheck;
            CenterCd = centerCd;
            Nyubi = nyubi;
            Yoketu = yoketu;
            Bilirubin = bilirubin;
            IsDeleted = isDeleted;
        }

        public int HpId { get; private set; }
        public long PtId { get; private set; }
        public long IraiCd { get; private set; }
        public int IraiDate { get; private set; }
        public long RaiinNo { get; private set; }
        public int InoutKbn { get; private set; }
        public int Status { get; private set; }
        public int TosekiKbn { get; private set; }
        public int SikyuKbn { get; private set; }
        public int ResultCheck { get; private set; }
        public string CenterCd { get; private set; }
        public string Nyubi { get; private set; }
        public string Yoketu { get; private set; }
        public string Bilirubin { get; private set; }
        public int IsDeleted { get; private set; }

    }
}
