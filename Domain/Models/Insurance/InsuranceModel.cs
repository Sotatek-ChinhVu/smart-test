using Domain.Constant;
using Domain.Models.Insurance;
using Helper.Common;
using Helper.Constant;
using Helper.Constants;
using Helper.Extension;
using System.Text.Json.Serialization;

namespace Domain.Models.InsuranceInfor
{
    public class InsuranceModel
    {
        public InsuranceModel(int hpId, long ptId, int ptBirthDay, long seqNo, int hokenSbtCd, int hokenPid, int hokenKbn, int sinDate, string memo, HokenInfModel hokenInf, KohiInfModel kohi1, KohiInfModel kohi2, KohiInfModel kohi3, KohiInfModel kohi4, int isDeleted, int startDate, int endDate)
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

        [JsonConstructor]
        public InsuranceModel(int hpId, long ptId, int ptBirthday, long seqNo, int hokenSbtCd, int hokenPid, int hokenKbn, string hokenMemo, int sinDate, int isDeleted, HokenInfModel hokenInf, KohiInfModel? kohi1, KohiInfModel? kohi2, KohiInfModel? kohi3, KohiInfModel? kohi4, int startDate, int endDate)
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
            IsDeleted = isDeleted;
            HokenInf = hokenInf;
            Kohi1 = kohi1 ?? new KohiInfModel(0);
            Kohi2 = kohi2 ?? new KohiInfModel(0);
            Kohi3 = kohi3 ?? new KohiInfModel(0);
            Kohi4 = kohi4 ?? new KohiInfModel(0);
            StartDate = startDate;
            EndDate = endDate;
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

        #region Expose properties

        public int StartDate { get; private set; }

        public int GenmenKbn => HokenInf == null ? 0 : HokenInf.GenmenKbn;

        public int GenmenRate => HokenInf == null ? 0 : HokenInf.GenmenRate;

        public int GenmenGaku => HokenInf == null ? 0 : HokenInf.GenmenGaku;

        public int SyokumuKbn => HokenInf == null ? 0 : HokenInf.SyokumuKbn;

        public int KeizokuKbn => HokenInf == null ? 0 : HokenInf.KeizokuKbn;

        public string Tokki1 => HokenInf == null ? string.Empty : HokenInf.Tokki1;

        public string Tokki2 => HokenInf == null ? string.Empty : HokenInf.Tokki2;

        public string Tokki3 => HokenInf == null ? string.Empty : HokenInf.Tokki3;

        public string Tokki4 => HokenInf == null ? string.Empty : HokenInf.Tokki4;

        public string Tokki5 => HokenInf == null ? string.Empty : HokenInf.Tokki5;
        //2
        public string RousaiKofuNo => HokenInf == null ? string.Empty : HokenInf.RousaiKofuNo;

        public string NenkinBango => HokenInf == null ? string.Empty : HokenInf.NenkinBango;

        public string RousaiRoudouCd => HokenInf == null ? string.Empty : HokenInf.RousaiRoudouCd;

        public string KenkoKanriBango => HokenInf == null ? string.Empty : HokenInf.KenkoKanriBango;

        public int RousaiSaigaiKbn => HokenInf == null ? 0 : HokenInf.RousaiSaigaiKbn;

        public string RousaiKantokuCd => HokenInf == null ? string.Empty : HokenInf.RousaiKantokuCd;

        public int RousaiSyobyoDate => HokenInf == null ? 0 : HokenInf.RousaiSyobyoDate;

        public int RyoyoStartDate => HokenInf == null ? 0 : HokenInf.RyoyoStartDate;

        public int RyoyoEndDate => HokenInf == null ? 0 : HokenInf.RyoyoEndDate;

        public string RousaiSyobyoCd => HokenInf == null ? string.Empty : HokenInf.RousaiSyobyoCd;

        public string RousaiJigyosyoName => HokenInf == null ? string.Empty : HokenInf.RousaiJigyosyoName;

        public string RousaiPrefName => HokenInf == null ? string.Empty : HokenInf.RousaiPrefName;

        public string RousaiCityName => HokenInf == null ? string.Empty : HokenInf.RousaiCityName;

        public int RousaiReceCount => HokenInf == null ? 0 : HokenInf.RousaiReceCount;

        public string JibaiHokenName => HokenInf == null ? string.Empty : HokenInf.JibaiHokenName;

        public string JibaiHokenTanto => HokenInf == null ? string.Empty : HokenInf.JibaiHokenTanto;

        public string JibaiHokenTel => HokenInf == null ? string.Empty : HokenInf.JibaiHokenTel;

        public string HokensyaName => HokenInf == null ? string.Empty : HokenInf.HokensyaName;

        public string HokensyaAddress => HokenInf == null ? string.Empty : HokenInf.HokensyaAddress;

        public string HokensyaTel => HokenInf == null ? string.Empty : HokenInf.HokensyaTel;

        public int JibaiJyusyouDate => HokenInf == null ? 0 : HokenInf.JibaiJyusyouDate;

        public int FutanKbn => HokenInf != null ? HokenInf.HokenMstFutanKbn : 0;
        public int EndDate { get; private set; }

        public string DisplayRateOnly => GetRateOnly(PtBirthday);

        public string HokenName => GetHokenName();

        public bool IsEmptyHoken => (HokenInf.HokenId == 0);

        public bool IsEmptyKohi1 => (Kohi1 == null || Kohi1.HokenId == 0);

        public bool IsEmptyKohi2 => (Kohi2 == null || Kohi2.HokenId == 0);

        public bool IsEmptyKohi3 => (Kohi3 == null || Kohi3.HokenId == 0);

        public bool IsEmptyKohi4 => (Kohi4 == null || Kohi4.HokenId == 0);

        public string PatternRate => GetHokenRate();

        public string HokensyaNo => HokenInf == null ? string.Empty : HokenInf.HokensyaNo;

        public string Kigo => HokenInf == null ? string.Empty : HokenInf.Kigo;

        public string Bango => HokenInf == null ? string.Empty : HokenInf.Bango;

        public string EdaNo => HokenInf == null ? string.Empty : HokenInf.EdaNo;

        public int SikakuDate => HokenInf == null ? 0 : HokenInf.SikakuDate;

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