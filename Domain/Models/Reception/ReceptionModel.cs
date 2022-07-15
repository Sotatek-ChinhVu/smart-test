using Domain.CommonObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Reception
{
    public class ReceptionModel
    {
        public HpId HpId { get; private set; }

        public PtId PtId { get; private set; }

        public SinDate SinDate { get; private set; }

        public RaiinNo RaiinNo { get; private set; }

        public OyaRaiinNo OyaRaiinNo { get; private set; }

        public HokenPid HokenPid { get; private set; }

        public int SanteiKbn { get; private set; }

        public int Status { get; private set; }

        public int IsYoyaku { get; private set; }

        public string YoyakuTime { get; private set; }

        public int YoyakuId { get; private set; }

        public int UketukeSbt { get; private set; }

        public string UketukeTime { get; private set; }

        public int UketukeId { get; private set; }

        public int UketukeNo { get; private set; }

        public string SinStartTime { get; private set; }

        public string SinEndTime { get; private set; }

        public string KaikeiTime { get; private set; }

        public int KaikeiId { get; private set; }

        public int KaId { get; private set; }

        public int TantoId { get; private set; }

        public int SyosaisinKbn { get; private set; }

        public int JikanKbn { get; private set; }

        public ReceptionModel(HpId hpId, PtId ptId, SinDate sinDate, RaiinNo raiinNo, OyaRaiinNo oyaRaiinNo, HokenPid hokenPid, int santeiKbn, int status, int isYoyaku, string yoyakuTime, int yoyakuId, int uketukeSbt, string uketukeTime, int uketukeId, int uketukeNo, string sinStartTime, string sinEndTime, string kaikeiTime, int kaikeiId, int kaId, int tantoId, int syosaisinKbn, int jikanKbn)
        {
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
            RaiinNo = raiinNo;
            OyaRaiinNo = oyaRaiinNo;
            HokenPid = hokenPid;
            SanteiKbn = santeiKbn;
            Status = status;
            IsYoyaku = isYoyaku;
            YoyakuTime = yoyakuTime;
            YoyakuId = yoyakuId;
            UketukeSbt = uketukeSbt;
            UketukeTime = uketukeTime;
            UketukeId = uketukeId;
            UketukeNo = uketukeNo;
            SinStartTime = sinStartTime;
            SinEndTime = sinEndTime;
            KaikeiTime = kaikeiTime;
            KaikeiId = kaikeiId;
            KaId = kaId;
            TantoId = tantoId;
            SyosaisinKbn = syosaisinKbn;
            JikanKbn = jikanKbn;
        }
    }
}
