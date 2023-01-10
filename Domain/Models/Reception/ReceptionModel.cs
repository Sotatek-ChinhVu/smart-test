﻿using Domain.Constant;
using Domain.Models.ReceptionSameVisit;
using Helper.Common;
using Helper.Extendsions;
using System.Text.Json.Serialization;

namespace Domain.Models.Reception
{
    public class ReceptionModel
    {
        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int SinDate { get; private set; }

        public string SinDateLabel
        {
            get { return CIUtil.SDateToShowSDate(SinDate); }
        }

        public int UketukeNo { get; private set; }

        public string StatusLbl
        {
            get
            {
                if (CheckDefaultValue())
                {
                    return string.Empty;
                }
                string result = string.Empty;
                switch (Status)
                {
                    case 0:
                        result = "予約";
                        break;
                    case 1:
                        result = "";
                        break;
                    case 3:
                        result = "一時保存";
                        break;
                    case 5:
                        result = "計算";
                        break;
                    case 7:
                        result = "精算待ち";
                        break;
                    case 9:
                        result = "精算済";
                        break;
                    default:
                        break;
                }
                return result;
            }
        }

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
        }

        public ReceptionModel(int hpId, long ptId, long raiinNo, string comment)
        {
            HpId = hpId;
            PtId = ptId;
            RaiinNo = raiinNo;
            Comment = comment;
            YoyakuTime = String.Empty;
            UketukeTime = String.Empty;
            SinStartTime = String.Empty;
            SinEndTime = String.Empty;
            KaikeiTime = String.Empty;
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
            YoyakuTime = String.Empty;
            SinEndTime = String.Empty;
            KaikeiTime = String.Empty;
            Comment = String.Empty;
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
            YoyakuTime = String.Empty;
            YoyakuId = 0;
            UketukeSbt = 0;
            UketukeTime = String.Empty;
            UketukeId = 0;
            UketukeNo = 0;
            SinStartTime = String.Empty;
            SinEndTime = String.Empty;
            KaikeiTime = String.Empty;
            KaikeiId = 0;
            KaId = 0;
            TantoId = 0;
            SyosaisinKbn = 0;
            JikanKbn = 0;
            Comment = String.Empty;
        }

        public ReceptionModel(int tantoId, int kaId)
        {
            Comment = String.Empty;
            YoyakuTime = String.Empty;
            UketukeTime = String.Empty;
            SinStartTime = String.Empty;
            SinEndTime = String.Empty;
            KaikeiTime = String.Empty;
            KaId = kaId;
            TantoId = tantoId;
        }

        public ReceptionModel(int hpId, long ptId, int sinDate, int uketukeNo, int status, string kaSname, string sName, string houbetu, string hokensyaNo, int hokenKbn, int hokenId)
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


        public string HokenKbnName
        {
            get
            {
                string result = string.Empty;
                if (PtId == 0 && HokenPid == 0 && HpId == 0)
                {
                    return "";
                }

                if (PtId == 0 && HokenId == 0 && HpId == 0)
                {
                    result = "公費";
                    return result;
                }

                if (Houbetu == HokenConstant.HOUBETU_NASHI)
                {
                    result = "公費";
                    return result;
                }

                switch (HokenKbn)
                {
                    case 0:
                        result = "自費";
                        break;
                    case 1:
                        result = "社保";
                        break;
                    case 2:
                        if (HokensyaNo.Length == 8 &&
                            HokensyaNo.StartsWith("39"))
                        {
                            result = "後期";
                        }
                        else if (HokensyaNo.Length == 8 &&
                            HokensyaNo.StartsWith("67"))
                        {
                            result = "退職";
                        }
                        else
                        {
                            result = "国保";
                        }
                        break;
                    case 11:
                    case 12:
                    case 13:
                        result = "労災";
                        break;
                    case 14:
                        result = "自賠";
                        break;
                }
                return result;
            }
        }

        public bool CheckDefaultValue()
        {
            return PtId == 0 && SinDate == 0 && RaiinNo == 0;
        }
    }
}
