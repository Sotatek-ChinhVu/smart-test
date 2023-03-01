using Domain.Models.Accounting;
using Domain.Models.ReceptionSameVisit;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;

namespace Domain.Models.Reception
{
    public class ReceptionDto
    {

        public ReceptionDto(int hpId, long ptId, int sinDate, long raiinNo, long oyaRaiinNo, int hokenPid, int santeiKbn, int status, int isYoyaku, string yoyakuTime, int yoyakuId, int uketukeSbt, string uketukeTime, int uketukeId, int uketukeNo, string sinStartTime, string sinEndTime, string kaikeiTime, int kaikeiId, int kaId, int tantoId, int syosaisinKbn, int jikanKbn, string comment)
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

        public ReceptionDto(long raiinNo, int uketukeNo, string departmentSName, int personNumber, HokenPatternModel hokenPatternModel, List<KaikeiInfModel> kaikeiInfModels)
        {
            RaiinNo = raiinNo;
            UketukeNo = uketukeNo;
            DepartmentSName = departmentSName;
            PersonNumber = personNumber;
            HokenPatternModel = hokenPatternModel;
            KaikeiInfModels = kaikeiInfModels;
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

        public string DepartmentSName { get; private set; }

        public int PersonNumber { get; private set; }

        public HokenPatternModel HokenPatternModel { get; private set; }

        public List<KaikeiInfModel> KaikeiInfModels { get; private set; }

        public string RaiinBinding => RaiinNo > 0 ? GetRaiinName() : "(すべて)";

        public string PatternName => GetPatternName();

        private string GetRaiinName()
        {
            string name = DepartmentSName;
            string hokenText = PatternName;
            string receptionOder = UketukeNo.ToString("D4");
            if (!string.IsNullOrEmpty(hokenText))
            {
                name = receptionOder + " " + name + "【" + hokenText + "】";
            }

            return name;
        }

        public string GetPatternName()
        {
            int hokenId = HokenPatternModel?.HokenId ?? 0;

            var kaikeInf = KaikeiInfModels?.FirstOrDefault(item => item.HokenId == hokenId);

            if (kaikeInf == null)
                return string.Empty;

            string patternName = string.Empty;

            string hokenKbn = kaikeInf.HokenKbn.AsString().PadLeft(2, '0');
            string hokenSbtCd = kaikeInf.HokenSbtCd.AsString().PadRight(3, '0');
            string receSbt = kaikeInf.ReceSbt.AsString().PadRight(4, '0');
            int firstNum = receSbt[0].AsInteger();
            int secondNum = receSbt[1].AsInteger();
            int thirdNum = hokenSbtCd[2].AsInteger();
            int fourthNum;
            if ((firstNum == 1 && secondNum == 2)
                || (firstNum == 1 && secondNum == 3))
            {
                // In case ReceSbt = 12x2: 公費, fourthNum is always = 0
                // In case ReceSbt = 13x8 or 13x0: 後期, fourthNum is always = 0
                fourthNum = 0;
            }
            else
            {
                fourthNum = receSbt[3].AsInteger();
            }
            string key = hokenKbn + firstNum.AsString() + secondNum.AsString() + thirdNum.AsString() + fourthNum.AsString();

            if (HokenPatternConstant.PatternNameDic.ContainsKey(key) == true)
            {
                patternName = HokenPatternConstant.PatternNameDic[key];
            }
            if (kaikeInf.HokenKbn == 0)
            {
                int kohiCount = GetKohiCount(kaikeInf);
                if (kohiCount > 0)
                {
                    patternName += GetKohiCountName(kohiCount);
                }
            }
            if (string.IsNullOrWhiteSpace(patternName) == true)
            {
                patternName = "自費レセ100％";
            }
            else
            {
                patternName = string.Format($"{patternName} {kaikeInf.DispRate}％");
            }

            return patternName;
        }

        private int GetKohiCount(KaikeiInfModel kaikeInf)
        {
            if (kaikeInf == null)
            {
                return 0;
            }
            int result = 0;
            if (kaikeInf.Kohi1Id > 0 && kaikeInf.Kohi1Houbetu != HokenConstant.HOUBETU_MARUCHO)
            {
                result++;
            }
            if (kaikeInf.Kohi2Id > 0 && kaikeInf.Kohi2Houbetu != HokenConstant.HOUBETU_MARUCHO)
            {
                result++;
            }
            if (kaikeInf.Kohi3Id > 0 && kaikeInf.Kohi3Houbetu != HokenConstant.HOUBETU_MARUCHO)
            {
                result++;
            }
            if (kaikeInf.Kohi4Id > 0 && kaikeInf.Kohi4Houbetu != HokenConstant.HOUBETU_MARUCHO)
            {
                result++;
            }
            if (result > 0)
            {
                return result + 1;
            }
            return result;
        }

        private string GetKohiCountName(int kohicount)
        {
            if (kohicount <= 0)
            {
                return string.Empty;
            }
            if (kohicount == 1)
            {
                return "単独";
            }
            else
            {
                return HenkanJ.HankToZen(kohicount.AsString()) + "併";
            }
        }

        public ReceptionDto()
        {
            YoyakuTime = string.Empty;
            UketukeTime = string.Empty;
            SinStartTime = string.Empty;
            SinEndTime = string.Empty;
            KaikeiTime = string.Empty;
            Comment = string.Empty;
            DepartmentSName = string.Empty;
            KaikeiInfModels = new();
        }
    }
}
