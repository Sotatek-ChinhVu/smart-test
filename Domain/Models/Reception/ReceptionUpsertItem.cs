﻿namespace Domain.Models.Reception
{
    public class ReceptionUpsertItem
    {
        public ReceptionUpsertItem(int hpId, long ptId, int sinDate, long raiinNo, long oyaRaiinNo, int hokenPid, int santeiKbn, int status, int isYoyaku, string yoyakuTime, int yoyakuId, int uketukeSbt, string uketukeTime, int uketukeId, int uketukeNo, string sinStartTime, string sinEndTime, string kaikeiTime, int kaikeiId, int kaId, int tantoId, int syosaisinKbn, int jikanKbn, string comment)
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
            Comment = comment;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int SinDate { get; private set; }

        public long RaiinNo { get; private set; }

        public long OyaRaiinNo { get; private set; }

        public int HokenPid { get; private set; }

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

        public string Comment { get; private set; }

        public ReceptionUpsertItem ChangeUketukeNo(int uketukeNo)
        {
            UketukeNo = uketukeNo;
            return this;
        }
    }
}
