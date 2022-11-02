using Domain.Constant;
using Domain.Models.Insurance;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Models.InsuranceInfor
{
    public class InsuranceModel
    {
        public InsuranceModel(int hpId, long ptId, int ptBirthDay, long seqNo, int hokenSbtCd, int hokenPid, int hokenKbn, int sinDate, string memo, HokenInfModel hokenInf, KohiInfModel kohi1, KohiInfModel kohi2, KohiInfModel kohi3, KohiInfModel kohi4, int isDeleted, int startDate, int endDate, bool isAddNew)
        {
            HpId = hpId;
            PtId = ptId;
            PtBirthday = ptBirthDay;
            SeqNo = seqNo;
            HokenSbtCd = hokenSbtCd;
            HokenPid = hokenPid;
            HokenKbn = hokenKbn;
            SinDate = sinDate;
            HokenMemo = memo;
            HokenInf = hokenInf;
            Kohi1 = kohi1;
            Kohi2 = kohi2;
            Kohi3 = kohi3;
            Kohi4 = kohi4;
            IsDeleted = isDeleted;
            StartDate = startDate;
            EndDate = endDate;
            IsAddNew = isAddNew;
        }

        public InsuranceModel() // new model
        {
            HpId = 0;
            PtId = 0;
            PtBirthday = 0;
            SeqNo = 0;
            HokenSbtCd = 0;
            HokenPid = 0;
            HokenKbn = 0;
            SinDate = 0;
            HokenMemo = string.Empty;
            HokenInf = new HokenInfModel(0, 0, 0);
            Kohi1 = new KohiInfModel(0);
            Kohi2 = new KohiInfModel(0);
            Kohi3 = new KohiInfModel(0);
            Kohi4 = new KohiInfModel(0);
        }

        public InsuranceModel(int hpId, long ptId, long seqNo, int hokenSbtCd, int hokenPid, int hokenKbn, int hokenId, int kohi1Id, int kohi2Id, int kohi3Id, int kohi4Id, int startDate, int endDate)
        {
            HpId = hpId;
            PtId = ptId;
            PtBirthday = 0;
            SeqNo = seqNo;
            HokenSbtCd = hokenSbtCd;
            HokenPid = hokenPid;
            HokenKbn = hokenKbn;
            SinDate = 0;
            HokenMemo = string.Empty;
            HokenInf = new HokenInfModel(hokenId, startDate, endDate);
            Kohi1 = new KohiInfModel(kohi1Id);
            Kohi2 = new KohiInfModel(kohi2Id);
            Kohi3 = new KohiInfModel(kohi3Id);
            Kohi4 = new KohiInfModel(kohi4Id);
            StartDate = startDate;
            EndDate = endDate;
        }

        public InsuranceModel(int hpId, long ptId, int ptBirthday, long seqNo, int hokenSbtCd, int hokenPid, int hokenKbn, string hokenMemo, int sinDate, int startDate, int endDate, int hokenId, int kohi1Id, int kohi2Id, int kohi3Id, int kohi4Id, bool isAddNew, int isDeleted)
        {
            HpId = hpId;
            PtId = ptId;
            PtBirthday = ptBirthday;
            SeqNo = seqNo;
            HokenSbtCd = hokenSbtCd;
            HokenPid = hokenPid;
            HokenKbn = hokenKbn;
            HokenMemo = hokenMemo;
            SinDate = sinDate;
            IsDeleted = 0;
            HokenInf = new HokenInfModel(hokenId, startDate, endDate);
            Kohi1 = new KohiInfModel(kohi1Id);
            Kohi2 = new KohiInfModel(kohi2Id);
            Kohi3 = new KohiInfModel(kohi3Id);
            Kohi4 = new KohiInfModel(kohi4Id);
            StartDate = startDate;
            EndDate = endDate;
            HokenId = hokenId;
            Kohi1Id = kohi1Id;
            Kohi2Id = kohi2Id;
            Kohi3Id = kohi3Id;
            Kohi4Id = kohi4Id;
            IsAddNew = isAddNew;
            IsDeleted = isDeleted;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int PtBirthday { get; private set; }

        public long SeqNo { get; private set; }

        public int HokenSbtCd { get; private set; }

        public int HokenPid { get; private set; }

        public int HokenKbn { get; private set; }

        public string HokenMemo { get; private set; }

        public int SinDate { get; private set; }

        public int IsDeleted { get; private set; }

        public HokenInfModel HokenInf { get; private set; }

        public KohiInfModel Kohi1 { get; private set; }

        public KohiInfModel Kohi2 { get; private set; }

        public KohiInfModel Kohi3 { get; private set; }

        public KohiInfModel Kohi4 { get; private set; }

        public int HoubetuPoint(List<string> houbetuList)
        {
            int point = 0;
            if (!IsEmptyHoken && !HokenInf.IsNoHoken) point++;
            if (!IsEmptyKohi1 && houbetuList.Contains(Kohi1.Houbetu)) point++;
            if (!IsEmptyKohi2 && houbetuList.Contains(Kohi2.Houbetu)) point++;
            if (!IsEmptyKohi3 && houbetuList.Contains(Kohi3.Houbetu)) point++;
            if (!IsEmptyKohi4 && houbetuList.Contains(Kohi4.Houbetu)) point++;
            return point;
        }

        public int KohiCount
        {
            get
            {
                int count = 0;
                if (!IsEmptyKohi1) count++;
                if (!IsEmptyKohi2) count++;
                if (!IsEmptyKohi3) count++;
                if (!IsEmptyKohi4) count++;
                return count;
            }
        }

        public List<KohiInfModel> BuntenKohis
        {
            get
            {
                var result = new List<KohiInfModel>();
                if (!IsEmptyKohi1 && Kohi1.HokenSbtKbn == 6) result.Add(Kohi1);
                if (!IsEmptyKohi2 && Kohi2.HokenSbtKbn == 6) result.Add(Kohi2);
                if (!IsEmptyKohi3 && Kohi3.HokenSbtKbn == 6) result.Add(Kohi3);
                if (!IsEmptyKohi4 && Kohi4.HokenSbtKbn == 6) result.Add(Kohi4);
                return result;
            }
        }

        #region Expose properties

        public int StartDate { get; private set; }

        public int EndDate { get; private set; }

        public int HokenId { get; private set; }

        public int Kohi1Id { get; private set; }

        public int Kohi2Id { get; private set; }

        public int Kohi3Id { get; private set; }

        public int Kohi4Id { get; private set; }

        public string DisplayRateOnly => GetRateOnly(PtBirthday);

        public string HokenName => GetHokenName();

        public bool IsEmptyHoken => (HokenInf.HokenId == 0);

        public bool IsEmptyKohi1 => (Kohi1 == null || Kohi1.HokenId == 0);

        public bool IsEmptyKohi2 => (Kohi2 == null || Kohi2.HokenId == 0);

        public bool IsEmptyKohi3 => (Kohi3 == null || Kohi3.HokenId == 0);

        public bool IsEmptyKohi4 => (Kohi4 == null || Kohi4.HokenId == 0);

        public string PatternRate => GetHokenRate();

        public bool IsShaho
        {
            // not nashi
            get => HokenKbn == 1 && HokenInf.Houbetu != HokenConstant.HOUBETU_NASHI;
        }

        public bool IsKokuho
        {
            get => HokenKbn == 2;
        }

        public bool IsNoHoken
        {
            get
            {
                if (HokenInf != null)
                {
                    return HokenInf.HokenMst.HokenSbtKbn == 0;
                }
                return HokenKbn == 1 && HokenInf?.Houbetu == HokenConstant.HOUBETU_NASHI;
            }
        }

        public bool IsJibaiOrRosai
        {
            get { return HokenKbn >= 11 && HokenKbn <= 14; }
        }

        public bool IsAddNew { get; private set; }

        public bool IsExpirated => !(StartDate <= SinDate && EndDate >= SinDate);
        #endregion

        #region Function

        public string GetHokenName()
        {
            string hokenName = HokenPid.ToString().PadLeft(3, '0') + ". ";

            if (!(HokenInf.StartDate <= SinDate && HokenInf.EndDate >= SinDate))
            {
                hokenName = "×" + hokenName;
            }

            string prefix = string.Empty;
            string postfix = string.Empty;
            if (HokenSbtCd == 0)
            {
                switch (HokenKbn)
                {
                    case 0:
                        if (!string.IsNullOrWhiteSpace(HokenInf.HokenMstHoubetu))
                        {
                            if (HokenInf.HokenMstHoubetu == HokenConstant.HOUBETU_JIHI_108)
                            {
                                hokenName += "自費 " + HokenInf.HokenMstFutanRate + "%";
                            }
                            else if (HokenInf.HokenMstHoubetu == HokenConstant.HOUBETU_JIHI_109)
                            {
                                hokenName += "自費レセ " + HokenInf.HokenMstFutanRate + "%";
                            }
                            else
                            {
                                hokenName += "自費";
                            }
                        }
                        else
                        {
                            hokenName += "自費";
                        }
                        break;
                    case 11:
                        hokenName += "労災（短期給付）";
                        break;
                    case 12:
                        hokenName += "労災（傷病年金）";
                        break;
                    case 13:
                        hokenName += "労災（アフターケア）";
                        break;
                    case 14:
                        hokenName += "自賠責";
                        break;
                    default:
                        break;
                }
            }
            else
            {
                if (HokenSbtCd < 0)
                {
                    return hokenName;
                }
                string subHokenSbtCd = HokenSbtCd.ToString().PadRight(3, '0');
                int firstNum = Int32.Parse(subHokenSbtCd[0].ToString());
                int secondNum = Int32.Parse(subHokenSbtCd[1].ToString());
                int thirNum = Int32.Parse(subHokenSbtCd[2].ToString());
                switch (firstNum)
                {
                    case 1:
                        hokenName += "社保";
                        break;
                    case 2:
                        hokenName += "国保";
                        break;
                    case 3:
                        hokenName += "後期";
                        break;
                    case 4:
                        hokenName += "退職";
                        break;
                    case 5:
                        hokenName += "公費";
                        break;
                }

                if (secondNum > 0)
                {

                    if (thirNum == 1)
                    {
                        prefix += "単独";
                    }
                    else
                    {
                        prefix += thirNum + "併";
                    }

                    if (Kohi1 != null)
                    {
                        if (!string.IsNullOrEmpty(postfix))
                        {
                            postfix += "+";
                        }
                        if (Kohi1.HokenSbtKbn != 2)
                        {
                            postfix += Kohi1.Houbetu;
                        }
                        else
                        {
                            postfix += "マル長";
                        }
                    }
                    if (Kohi2 != null)
                    {
                        if (!string.IsNullOrEmpty(postfix))
                        {
                            postfix += "+";
                        }
                        if (Kohi2.HokenSbtKbn != 2)
                        {
                            postfix += Kohi2.Houbetu;
                        }
                        else
                        {
                            postfix += "マル長";
                        }
                    }
                    if (Kohi3 != null)
                    {
                        if (!string.IsNullOrEmpty(postfix))
                        {
                            postfix += "+";
                        }
                        if (Kohi3.HokenSbtKbn != 2)
                        {
                            postfix += Kohi3.Houbetu;
                        }
                        else
                        {
                            postfix += "マル長";
                        }
                    }
                    if (Kohi4 != null)
                    {
                        if (!string.IsNullOrEmpty(postfix))
                        {
                            postfix += "+";
                        }
                        if (Kohi4.HokenSbtKbn != 2)
                        {
                            postfix += Kohi4.Houbetu;
                        }
                        else
                        {
                            postfix += "マル長";
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(postfix))
            {
                return hokenName + prefix + "(" + postfix + ")";
            }
            return hokenName + prefix;
        }

        private string GetRateOnly(int birthDay)
        {
            string resultRate = string.Empty;
            int rate = 0;
            if (HokenSbtCd < 0)
            {
                return resultRate;
            }

            if (HokenSbtCd == 0)
            {
                switch (HokenKbn)
                {
                    case 0:
                        if (!IsEmptyHoken && HokenInf.HokenMstFutanRate != 0)
                        {
                            if (HokenInf.HokenMstHoubetu == HokenConstant.HOUBETU_JIHI_108)
                            {
                                resultRate += HokenInf.HokenMstFutanRate + "%";
                            }
                            else if (HokenInf.HokenMstHoubetu == HokenConstant.HOUBETU_JIHI_109)
                            {
                                resultRate += HokenInf.HokenMstFutanRate + "%";
                            }
                        }
                        return resultRate;
                    case 11:
                    case 12:
                    case 13:
                    case 14:
                        if (!IsEmptyHoken && HokenInf.HokenMstFutanRate != 0)
                        {
                            resultRate += HokenInf.HokenMstFutanRate + "%";
                        }
                        return resultRate;
                }
            }
            else
            {
                string hokenSbtCd = HokenSbtCd.AsString().PadRight(3, '0');
                int firstNum = hokenSbtCd[0].AsInteger();
                bool isHoken39 = firstNum == 3;

                rate = GetRate(isHoken39, birthDay);
            }

            resultRate += rate + "%";
            return resultRate;
        }

        private int GetRate(bool isHoken39, int birthDay)
        {
            bool hasRateCome = false;
            int rateReturn = 0;
            if (!IsEmptyHoken
                && HokenInf.HokenMstHoubetu != null
                && HokenInf.HokenMstHoubetu != HokenConstant.HOUBETU_NASHI)
            {
                rateReturn = GetRateHoken(isHoken39, birthDay);
                hasRateCome = true;
            }
            if (!IsEmptyKohi1 && Kohi1.HokenMstModel != null)
            {
                if (Kohi1.HokenMstModel.FutanKbn == 0)
                {
                    rateReturn = 0;
                    hasRateCome = true;
                }
                else
                {
                    if (Kohi1.HokenMstModel.FutanRate > 0)
                    {
                        if (hasRateCome)
                        {
                            rateReturn = Math.Min(rateReturn, Kohi1.HokenMstModel.FutanRate);
                        }
                        else
                        {
                            rateReturn = Kohi1.HokenMstModel.FutanRate;
                        }
                        hasRateCome = true;
                    }
                }
            }
            if (!IsEmptyKohi2 && Kohi2.HokenMstModel != null)
            {
                if (Kohi2.HokenMstModel.FutanKbn == 0)
                {
                    rateReturn = 0;
                    hasRateCome = true;
                }
                else
                {
                    if (Kohi2.HokenMstModel.FutanRate > 0)
                    {
                        if (hasRateCome)
                        {
                            rateReturn = Math.Min(rateReturn, Kohi2.HokenMstModel.FutanRate);
                        }
                        else
                        {
                            rateReturn = Kohi2.HokenMstModel.FutanRate;
                        }
                        hasRateCome = true;
                    }
                }
            }
            if (!IsEmptyKohi3 && Kohi3.HokenMstModel != null)
            {
                if (Kohi3.HokenMstModel.FutanKbn == 0)
                {
                    rateReturn = 0;
                    hasRateCome = true;
                }
                else
                {
                    if (Kohi3.HokenMstModel.FutanRate > 0)
                    {
                        if (hasRateCome)
                        {
                            rateReturn = Math.Min(rateReturn, Kohi3.HokenMstModel.FutanRate);
                        }
                        else
                        {
                            rateReturn = Kohi3.HokenMstModel.FutanRate;
                        }
                        hasRateCome = true;
                    }
                }
            }
            if (!IsEmptyKohi4 && Kohi4.HokenMstModel != null)
            {
                if (Kohi4.HokenMstModel.FutanKbn == 0)
                {
                    rateReturn = 0;
                }
                else
                {
                    if (Kohi4.HokenMstModel.FutanRate > 0)
                    {
                        if (hasRateCome)
                        {
                            rateReturn = Math.Min(rateReturn, Kohi4.HokenMstModel.FutanRate);
                        }
                        else
                        {
                            rateReturn = Kohi4.HokenMstModel.FutanRate;
                        }
                    }
                }
            }
            return rateReturn;
        }

        private int GetRateHoken(bool isHoken39, int birthday)
        {
            if (!IsEmptyHoken && HokenInf.HokenMstFutanRate != 0)
            {
                int rateMst1 = HokenInf.HokenMstFutanRate;
                int rateMst2 = 0;
                if (HokenKbn == 1 || HokenKbn == 2)
                {
                    // 未就学児:
                    if (IsPreSchool(birthday))
                    {
                        rateMst2 = 20;
                    }
                    //後期:
                    else if (isHoken39)
                    {
                        if (HokenInf.KogakuKbn == 3
                            || (new List<int>() { 26, 27, 28 }.Contains(HokenInf.KogakuKbn)))
                        {
                            return 30;
                        }
                        else
                        {
                            rateMst2 = HokenInf.HokenMstFutanRate;  // 10%
                        }
                    }
                    // 高齢:
                    else if (IsElder(birthday))
                    {
                        if (HokenInf.KogakuKbn == 3
                            || (new List<int>() { 26, 27, 28 }.Contains(HokenInf.KogakuKbn)))
                        {
                            return 30;
                            //rateMst2 = HokenInf.HokenMasterModel.FutanRate;  // 30%
                        }
                        else if (Age(birthday) >= 75 && SinDate >= KaiseiDate.d20140501)
                        {
                            rateMst2 = 20;
                        }
                        else if (CIUtil.Is70Zenki_20per(birthday, SinDate))
                        {
                            rateMst2 = 20;
                        }
                        else
                        {
                            rateMst2 = 10;
                        }
                    }
                    else
                    {
                        rateMst2 = rateMst1;
                    }
                }
                return Math.Min(rateMst1, rateMst2);
            }
            return 0;
        }

        private bool IsPreSchool(int birthday)
        {
            return !CIUtil.IsStudent(birthday, SinDate);
        }

        private bool IsElder(int birthday)
        {
            return CIUtil.AgeChk(birthday, SinDate, 70);
        }

        private int Age(int birthday)
        {
            return CIUtil.SDateToAge(birthday, SinDate);
        }


        private string GetHokenRate()
        {
            string hokenName = string.Empty;
            bool isHoken39 = false;
            bool isKohiOnly = false;
            int rate = 0;
            if (this.HokenSbtCd < 0)
            {
                return hokenName;
            }

            string prefix = string.Empty;
            if (this.HokenSbtCd == 0)
            {
                switch (this.HokenKbn)
                {
                    case 0:
                        if (!IsEmptyHoken && HokenInf.HokenMst != null)
                        {
                            if (HokenInf.HokenMst.Houbetu == HokenConstant.HOUBETU_JIHI_109)
                            {
                                hokenName += "自費レセ " + HokenInf.HokenMst.FutanRate + "%";
                            }
                            else
                            {
                                hokenName += "自費 " + HokenInf.HokenMst.FutanRate + "%";
                            }
                        }
                        return hokenName;
                    case 11:
                        hokenName += "労災（短期給付）";
                        break;
                    case 12:
                        hokenName += "労災（傷病年金）";
                        break;
                    case 13:
                        hokenName += "労災（アフターケア）";
                        break;
                    case 14:
                        hokenName += "自賠責";
                        break;
                }

                if (!string.IsNullOrEmpty(hokenName) && (HokenInf != null && HokenInf.HokenMst != null))
                {
                    hokenName += " " + HokenInf.HokenMst.FutanRate + "%";
                }

                return hokenName;
            }
            else
            {
                string hokenSbtCd = this.HokenSbtCd.AsString().PadRight(3, '0');
                int firstNum = hokenSbtCd[0].AsInteger();
                int secondNum = hokenSbtCd[1].AsInteger();
                int thirNum = hokenSbtCd[2].AsInteger();
                switch (firstNum)
                {
                    case 1:
                        hokenName += "社保";
                        break;
                    case 2:
                        hokenName += "国保";
                        break;
                    case 3:
                        hokenName += "後期";
                        isHoken39 = true;
                        break;
                    case 4:
                        hokenName += "退職";
                        break;
                    case 5:
                        hokenName += "公費";
                        isKohiOnly = true;
                        break;
                }

                if (secondNum > 0)
                {

                    if (thirNum == 1)
                    {
                        prefix += "単独";
                    }
                    else
                    {
                        prefix += HenkanJ.HankToZen(thirNum.AsString()) + "併";
                    }
                }

                rate = GetRate(isHoken39, this.PtBirthday);
            }
            string name = hokenName + prefix;
            if (!isHoken39 && !isKohiOnly)
            {
                if (IsPreSchool(this.PtBirthday, this.SinDate))
                {
                    name += "・６未";
                }
                else if (IsElder(this.PtBirthday, this.SinDate))
                {
                    name += "・高齢";
                }
                else
                {
                    if (!IsEmptyHoken)
                    {
                        if (HokenInf.HonkeKbn == 1)
                        {
                            name += "・本人";
                        }
                        else if (HokenInf.HonkeKbn == 2)
                        {
                            name += "・家族";
                        }
                    }
                }
            }

            name += " " + rate + "%";
            return name;
        }

        private bool IsPreSchool(int birthDay, int sinDate)
        {
            return !CIUtil.IsStudent(birthDay, sinDate);
        }

        private bool IsElder(int birthDay, int sinDate)
        {
            return CIUtil.AgeChk(birthDay, sinDate, 70);
        }
        #endregion
    }
}