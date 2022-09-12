using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ReceptionVisitingModel
{
    public class ReceptionVisitingModel
    {
        public ReceptionVisitingModel(long ptId, int uketukeNo, int kaId, string uketukeTime, int status, int yokakuId, int tantoId)
        {
            PtId = ptId;
            UketukeNo = uketukeNo;
            KaId = kaId;
            UketukeTime = uketukeTime;
            Status = status;
            YoyakuId = yokakuId;
            TanToId = tantoId;
        }

        public long PtId { get; private set; }

        public int UketukeNo { get; private set; }

        public int KaId { get; private set; }

        public string UketukeTime { get; private set; }

        public int Status { get; private set; }

        public int YoyakuId { get; private set; }

        public int TanToId { get; private set; }
    }
}
