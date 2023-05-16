﻿using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace Domain.Models.Reception
{
    public class ReceptionModel
    {
        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int SinDate { get; private set; }

        public int UketukeNo { get; private set; }

        public string SName { get; private set; }

        public string KaSname { get; private set; }

        public string Houbetu { get; private set; }

        public string HokensyaNo { get; private set; }

        public int HokenKbn { get; private set; }

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

        public string SinStartTime { get; private set; }

        public string SinEndTime { get; private set; }

        public string KaikeiTime { get; private set; }

        public int KaikeiId { get; private set; }

        public int KaId { get; private set; }

        public int TantoId { get; private set; }

        public int SyosaisinKbn { get; private set; }

        public int JikanKbn { get; private set; }

        public string Comment { get; private set; }

        public int HokenId { get; private set; }

        public bool IsDeleted { get; private set; }

        public string HokenKbnName { get; private set; }

        [JsonConstructor]
        public ReceptionModel(int hpId, long ptId, int sinDate, long raiinNo, long oyaRaiinNo, int hokenPid, int santeiKbn, int status, int isYoyaku, string yoyakuTime, int yoyakuId, int uketukeSbt, string uketukeTime, int uketukeId, int uketukeNo, string sinStartTime, string sinEndTime, string kaikeiTime, int kaikeiId, int kaId, int tantoId, int syosaisinKbn, int jikanKbn, string comment)
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
            SName = string.Empty;
            KaSname = string.Empty;
            Houbetu = string.Empty;
            HokensyaNo = string.Empty;
            HokenKbnName = string.Empty;
        }

        public ReceptionModel(int hpId, long ptId, long raiinNo, string comment)
        {
            HpId = hpId;
            PtId = ptId;
            RaiinNo = raiinNo;
            Comment = comment;
            YoyakuTime = string.Empty;
            UketukeTime = string.Empty;
            SinStartTime = string.Empty;
            SinEndTime = string.Empty;
            KaikeiTime = string.Empty;
            SName = string.Empty;
            KaSname = string.Empty;
            Houbetu = string.Empty;
            HokensyaNo = string.Empty;
            HokenKbnName = string.Empty;
        }

        public ReceptionModel(long raiinNo, int uketukeId, int kaId, string uketukeTime, string sinStartTime, int status, int yokakuId, int tantoId)
        {
            RaiinNo = raiinNo;
            UketukeId = uketukeId;
            KaId = kaId;
            UketukeTime = uketukeTime;
            SinStartTime = sinStartTime;
            Status = status;
            YoyakuId = yokakuId;
            TantoId = tantoId;
            YoyakuTime = string.Empty;
            SinEndTime = string.Empty;
            KaikeiTime = string.Empty;
            Comment = string.Empty;
            SName = string.Empty;
            KaSname = string.Empty;
            Houbetu = string.Empty;
            HokensyaNo = string.Empty;
            HokenKbnName = string.Empty;
        }

        public ReceptionModel()
        {
            HpId = 0;
            PtId = 0;
            SinDate = 0;
            RaiinNo = 0;
            OyaRaiinNo = 0;
            HokenPid = 0;
            SanteiKbn = 0;
            Status = 0;
            IsYoyaku = 0;
            YoyakuTime = string.Empty;
            YoyakuId = 0;
            UketukeSbt = 0;
            UketukeTime = string.Empty;
            UketukeId = 0;
            UketukeNo = 0;
            SinStartTime = string.Empty;
            SinEndTime = string.Empty;
            KaikeiTime = string.Empty;
            KaikeiId = 0;
            KaId = 0;
            TantoId = 0;
            SyosaisinKbn = 0;
            JikanKbn = 0;
            Comment = string.Empty;
            SName = string.Empty;
            KaSname = string.Empty;
            Houbetu = string.Empty;
            HokensyaNo = string.Empty;
            HokenKbnName = string.Empty;
        }

        public ReceptionModel(int tantoId, int kaId)
        {
            Comment = string.Empty;
            YoyakuTime = string.Empty;
            UketukeTime = string.Empty;
            SinStartTime = string.Empty;
            SinEndTime = string.Empty;
            KaikeiTime = string.Empty;
            KaId = kaId;
            TantoId = tantoId;
            KaSname = string.Empty;
            SName = string.Empty;
            Houbetu = string.Empty;
            HokensyaNo = string.Empty;
            HokenKbnName = string.Empty;
        }

        public ReceptionModel(int hpId, long ptId, int sinDate, int uketukeNo, int status, string kaSname, string sName, string houbetu, string hokensyaNo, int hokenKbn, int hokenId, int hokenPid, long raiinNo, bool isDeleted)
        {
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
            UketukeNo = uketukeNo;
            Status = status;
            KaSname = kaSname;
            SName = sName;
            Houbetu = houbetu;
            HokensyaNo = hokensyaNo;
            HokenKbn = hokenKbn;
            HokenId = hokenId;
            HokenPid = hokenPid;
            RaiinNo = raiinNo;
            IsDeleted = isDeleted;
            Comment = string.Empty;
            YoyakuTime = string.Empty;
            UketukeTime = string.Empty;
            SinStartTime = string.Empty;
            SinEndTime = string.Empty;
            KaikeiTime = string.Empty;
            HokenKbnName = string.Empty;
        }

        public ReceptionModel(ReceptionUpsertItem receptionUpsertItem)
        {
            HpId = receptionUpsertItem.HpId;
            PtId = receptionUpsertItem.PtId;
            SinDate = receptionUpsertItem.SinDate;
            RaiinNo = receptionUpsertItem.RaiinNo;
            OyaRaiinNo = receptionUpsertItem.OyaRaiinNo;
            HokenPid = receptionUpsertItem.HokenPid;
            SanteiKbn = receptionUpsertItem.SanteiKbn;
            Status = receptionUpsertItem.Status;
            IsYoyaku = receptionUpsertItem.IsYoyaku;
            YoyakuTime = receptionUpsertItem.YoyakuTime;
            YoyakuId = receptionUpsertItem.YoyakuId;
            UketukeSbt = receptionUpsertItem.UketukeSbt;
            UketukeTime = receptionUpsertItem.UketukeTime;
            UketukeId = receptionUpsertItem.UketukeId;
            UketukeNo = receptionUpsertItem.UketukeNo;
            SinStartTime = receptionUpsertItem.SinStartTime;
            SinEndTime = receptionUpsertItem.SinEndTime;
            KaikeiTime = receptionUpsertItem.KaikeiTime;
            KaikeiId = receptionUpsertItem.KaikeiId;
            KaId = receptionUpsertItem.KaId;
            TantoId = receptionUpsertItem.TantoId;
            SyosaisinKbn = receptionUpsertItem.SyosaisinKbn;
            JikanKbn = receptionUpsertItem.JikanKbn;
            Comment = receptionUpsertItem.Comment;
            SName = string.Empty;
            KaSname = string.Empty;
            Houbetu = string.Empty;
            HokensyaNo = string.Empty;
            HokenKbnName = string.Empty;
        }

        public ReceptionModel(long ptId, int sinDate, long raiinNo, int tantoId, int kaId, string sName, string kaSname, string hokenKbnName)
        {
            PtId = ptId;
            SinDate = sinDate;
            RaiinNo = raiinNo;
            SName = sName;
            KaSname = kaSname;
            TantoId = tantoId;
            KaId = kaId;
            HokenKbnName = hokenKbnName;
            Houbetu = string.Empty;
            HokensyaNo = string.Empty;
            YoyakuTime = string.Empty;
            UketukeTime = string.Empty;
            SinStartTime = string.Empty;
            SinEndTime = string.Empty;
            KaikeiTime = string.Empty;
            Comment = string.Empty;
        }

        public ReceptionDto ToDto()
        {
            return new ReceptionDto
                (
                    HpId,
                    PtId,
                    SinDate,
                    RaiinNo,
                    OyaRaiinNo,
                    HokenPid,
                    SanteiKbn,
                    Status,
                    IsYoyaku,
                    YoyakuTime,
                    YoyakuId,
                    UketukeSbt,
                    UketukeTime,
                    UketukeId,
                    UketukeNo,
                    SinStartTime,
                    SinEndTime,
                    KaikeiTime,
                    KaikeiId,
                    KaId,
                    TantoId,
                    SyosaisinKbn,
                    JikanKbn,
                    Comment
                );
        }

        public ReceptionModel ChangeUketukeNo(int uketukeNo)
        {
            UketukeNo = uketukeNo;
            return this;
        }
    }
}
