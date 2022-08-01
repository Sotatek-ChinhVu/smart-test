using Domain.Constant;
using Domain.Models.Insurance;
using Helper.Common;
using Helper.Constant;
using Helper.Extendsions;

namespace Domain.Models.InsuranceInfor
{
    public class InsuranceModel
    {
        public InsuranceModel(int hpId, long ptId, int hokenId, long seqNo, int hokenNo, int hokenEdaNo, int hokenSbtCd, int hokenPid, int hokenKbn, int kohi1Id, int kohi2Id, int kohi3Id, int kohi4Id, string hokensyaNo, string kigo, string bango, string edaNo, int honkeKbn, int startDate, int endDate, int sikakuDate, int kofuDate, int confirmDate, KohiInfModel kohi1, KohiInfModel kohi2, KohiInfModel kohi3, KohiInfModel kohi4, int kogakuKbn, int tasukaiYm, int tokureiYm1, int tokureiYm2, int genmenKbn, int genmenRate, int genmenGaku, int syokumuKbn, int keizokuKbn, string tokki1, string tokki2, string tokki3, string tokki4, string tokki5, string rousaiKofuNo, string nenkinBango, string rousaiRoudouCd, string kenkoKanriBango, int rousaiSaigaiKbn, string rousaiKantokuCd, int rousaiSyobyoDate, int ryoyoStartDate, int ryoyoEndDate, string rousaiSyobyoCd, string rousaiJigyosyoName, string rousaiPrefName, string rousaiCityName, int rousaiReceCount, int rousaiTenkiSinkei, int rousaiTenkiTenki, int rousaiTenkiEndDate, string houbetu, int futanRate, int sinDate, bool isHokenMstNotNull, int birthday)
        {
            HpId = hpId;
            PtId = ptId;
            HokenId = hokenId;
            SeqNo = seqNo;
            HokenNo = hokenNo;
            HokenEdaNo = hokenEdaNo;
            HokenSbtCd = hokenSbtCd;
            HokenPid = hokenPid;
            HokenKbn = hokenKbn;
            Kohi1Id = kohi1Id;
            Kohi2Id = kohi2Id;
            Kohi3Id = kohi3Id;
            Kohi4Id = kohi4Id;
            HokensyaNo = hokensyaNo;
            Kigo = kigo;
            Bango = bango;
            EdaNo = edaNo;
            HonkeKbn = honkeKbn;
            StartDate = startDate;
            EndDate = endDate;
            SikakuDate = sikakuDate;
            KofuDate = kofuDate;
            ConfirmDate = confirmDate;
            Kohi1 = kohi1;
            Kohi2 = kohi2;
            Kohi3 = kohi3;
            Kohi4 = kohi4;
            KogakuKbn = kogakuKbn;
            TasukaiYm = tasukaiYm;
            TokureiYm1 = tokureiYm1;
            TokureiYm2 = tokureiYm2;
            GenmenKbn = genmenKbn;
            GenmenRate = genmenRate;
            GenmenGaku = genmenGaku;
            SyokumuKbn = syokumuKbn;
            KeizokuKbn = keizokuKbn;
            Tokki1 = tokki1;
            Tokki2 = tokki2;
            Tokki3 = tokki3;
            Tokki4 = tokki4;
            Tokki5 = tokki5;
            RousaiKofuNo = rousaiKofuNo;
            NenkinBango = nenkinBango;
            RousaiRoudouCd = rousaiRoudouCd;
            KenkoKanriBango = kenkoKanriBango;
            RousaiSaigaiKbn = rousaiSaigaiKbn;
            RousaiKantokuCd = rousaiKantokuCd;
            RousaiSyobyoDate = rousaiSyobyoDate;
            RyoyoStartDate = ryoyoStartDate;
            RyoyoEndDate = ryoyoEndDate;
            RousaiSyobyoCd = rousaiSyobyoCd;
            RousaiJigyosyoName = rousaiJigyosyoName;
            RousaiPrefName = rousaiPrefName;
            RousaiCityName = rousaiCityName;
            RousaiReceCount = rousaiReceCount;
            RousaiTenkiSinkei = rousaiTenkiSinkei;
            RousaiTenkiTenki = rousaiTenkiTenki;
            RousaiTenkiEndDate = rousaiTenkiEndDate;
            HokenMstHoubetu = houbetu;
            HokenMstFutanRate = futanRate;
            SinDate = sinDate;
            IsHokenMstNotNull = isHokenMstNotNull;
            Birthday = birthday;
        }

        public InsuranceModel(int hpId, long ptId, int hokenPid, long seqNo, int hokenKbn, int hokenSbtCd, int hokenId, int kohi1Id, int kohi2Id, int kohi3Id, int kohi4Id, int startDate, int endDate)
        {
            HpId = hpId;
            PtId = ptId;
            HokenPid = hokenPid;
            SeqNo = seqNo;
            HokenKbn = hokenKbn;
            HokenSbtCd = hokenSbtCd;
            HokenId = hokenId;
            Kohi1Id = kohi1Id;
            Kohi2Id = kohi2Id;
            Kohi3Id = kohi3Id;
            Kohi4Id = kohi4Id;
            StartDate = startDate;
            EndDate = endDate;
            HokenNo = 0;
            HokenEdaNo = 0;
            HokensyaNo = String.Empty;
            Kigo = String.Empty;
            Bango = String.Empty;
            EdaNo = String.Empty;
            HonkeKbn = 0;
            SikakuDate = 0;
            KofuDate = 0;
            ConfirmDate = 0;
            Kohi1 = new KohiInfModel(string.Empty, string.Empty, 0, 0, 0, 0, 0, 0, 0, 0, string.Empty, 0, string.Empty, null);
            Kohi2 = new KohiInfModel(string.Empty, string.Empty, 0, 0, 0, 0, 0, 0, 0, 0, string.Empty, 0, string.Empty, null);
            Kohi3 = new KohiInfModel(string.Empty, string.Empty, 0, 0, 0, 0, 0, 0, 0, 0, string.Empty, 0, string.Empty, null);
            Kohi4 = new KohiInfModel(string.Empty, string.Empty, 0, 0, 0, 0, 0, 0, 0, 0, string.Empty, 0, string.Empty, null);
            KogakuKbn = 0;
            TasukaiYm = 0;
            TokureiYm1 = 0;
            TokureiYm2 = 0;
            GenmenKbn = 0;
            GenmenRate = 0;
            GenmenGaku = 0;
            SyokumuKbn = 0;
            KeizokuKbn = 0;
            Tokki1 = string.Empty;
            Tokki2 = string.Empty;
            Tokki3 = string.Empty;
            Tokki4 = string.Empty;
            Tokki5 = string.Empty;
            RousaiKofuNo = string.Empty;
            NenkinBango = string.Empty;
            RousaiRoudouCd = string.Empty;
            KenkoKanriBango = string.Empty;
            RousaiSaigaiKbn = 0;
            RousaiKantokuCd = string.Empty;
            RousaiSyobyoDate = 0;
            RyoyoStartDate = 0;
            RyoyoEndDate = 0;
            RousaiSyobyoCd = string.Empty;
            RousaiJigyosyoName = string.Empty;
            RousaiPrefName = string.Empty;
            RousaiCityName = string.Empty;
            RousaiReceCount = 0;
            RousaiTenkiSinkei = 0;
            RousaiTenkiTenki = 0;
            RousaiTenkiEndDate = 0;
            HokenMstHoubetu = string.Empty;
            HokenMstFutanRate = 0;
            SinDate = 0;
            IsHokenMstNotNull = true;
            Birthday = 0;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int HokenId { get; private set; }

        public long SeqNo { get; private set; }

        public int HokenNo { get; private set; }

        public int HokenEdaNo { get; private set; }

        public int HokenSbtCd { get; private set; }

        public int HokenPid { get; private set; }

        public int HokenKbn { get; private set; }

        public int Kohi1Id { get; private set; }

        public int Kohi2Id { get; private set; }

        public int Kohi3Id { get; private set; }

        public int Kohi4Id { get; private set; }

        public string HokenName
        {
            get => GetHokenName();
        }

        public string HokensyaNo { get; private set; }

        public string Kigo { get; private set; }

        public string Bango { get; private set; }

        public string EdaNo { get; private set; }

        public int HonkeKbn { get; private set; }

        public int StartDate { get; private set; }

        public int EndDate { get; private set; }

        public int SikakuDate { get; private set; }

        public int KofuDate { get; private set; }

        public int ConfirmDate { get; private set; }

        public KohiInfModel Kohi1 { get; private set; }

        public KohiInfModel Kohi2 { get; private set; }

        public KohiInfModel Kohi3 { get; private set; }

        public KohiInfModel Kohi4 { get; private set; }
        // detail 1
        public int KogakuKbn { get; private set; }

        public int TasukaiYm { get; private set; }

        public int TokureiYm1 { get; private set; }

        public int TokureiYm2 { get; private set; }

        public int GenmenKbn { get; private set; }

        public int GenmenRate { get; private set; }

        public int GenmenGaku { get; private set; }

        public int SyokumuKbn { get; private set; }

        public int KeizokuKbn { get; private set; }

        public string Tokki1 { get; private set; }

        public string Tokki2 { get; private set; }

        public string Tokki3 { get; private set; }

        public string Tokki4 { get; private set; }

        public string Tokki5 { get; private set; }
        //2
        public string RousaiKofuNo { get; private set; }

        public string NenkinBango { get; private set; }

        public string RousaiRoudouCd { get; private set; }

        public string KenkoKanriBango { get; private set; }

        public int RousaiSaigaiKbn { get; private set; }

        public string RousaiKantokuCd { get; private set; }

        public int RousaiSyobyoDate { get; private set; }

        public int RyoyoStartDate { get; private set; }

        public int RyoyoEndDate { get; private set; }

        public string RousaiSyobyoCd { get; private set; }

        public string RousaiJigyosyoName { get; private set; }

        public string RousaiPrefName { get; private set; }

        public string RousaiCityName { get; private set; }

        public int RousaiReceCount { get; private set; }

        public int RousaiTenkiSinkei { get; private set; }

        public int RousaiTenkiTenki { get; private set; }

        public int RousaiTenkiEndDate { get; private set; }

        public string HokenMstHoubetu { get; private set; }

        public int HokenMstFutanRate { get; private set; }

        public int SinDate { get; private set; }

        public int Birthday { get; private set; }

        public string DisplayRateOnly
        {
            get => GetRateOnly();
        }
        public bool IsEmptyHoken
        {
            get => (HokenId == 0);
        }
        public bool IsHokenMstNotNull
        {
            get; private set;
        }

        private string GetHokenName()
        {
            string hokenName = HokenPid.ToString().PadLeft(3, '0') + ". ";

            if (!(StartDate <= SinDate && EndDate >= SinDate))
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
                        if (!string.IsNullOrWhiteSpace(HokenMstHoubetu))
                        {
                            if (HokenMstHoubetu == HokenConstant.HOUBETU_JIHI_108)
                            {
                                hokenName += "自費 " + HokenMstFutanRate + "%";
                            }
                            else if (HokenMstHoubetu == HokenConstant.HOUBETU_JIHI_109)
                            {
                                hokenName += "自費レセ " + HokenMstFutanRate + "%";
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


        private string GetRateOnly()
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
                        if (!IsEmptyHoken && IsHokenMstNotNull)
                        {
                            if (HokenMstHoubetu == HokenConstant.HOUBETU_JIHI_108)
                            {
                                resultRate += HokenMstFutanRate + "%";
                            }
                            else if (HokenMstHoubetu == HokenConstant.HOUBETU_JIHI_109)
                            {
                                resultRate += HokenMstFutanRate + "%";
                            }
                        }
                        return resultRate;
                    case 11:
                    case 12:
                    case 13:
                    case 14:
                        if (!IsEmptyHoken && IsHokenMstNotNull)
                        {
                            resultRate += HokenMstFutanRate + "%";
                        }
                        return resultRate;
                }
            }
            else
            {
                string hokenSbtCd = HokenSbtCd.AsString().PadRight(3, '0');
                int firstNum = hokenSbtCd[0].AsInteger();
                bool isHoken39 = firstNum == 3;

                rate = GetRate(isHoken39);
            }

            resultRate += rate + "%";
            return resultRate;
        }
        private int GetRate(bool isHoken39)
        {
            bool hasRateCome = false;
            int rateReturn = 0;
            if (!IsEmptyHoken
                && HokenMstHoubetu != null
                && HokenMstHoubetu != HokenConstant.HOUBETU_NASHI)
            {
                rateReturn = GetRateHoken(isHoken39);
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

        private int GetRateHoken(bool isHoken39)
        {
            if (!IsEmptyHoken && IsHokenMstNotNull)
            {
                int rateMst1;
                if (HokenMstFutanRate == 0)
                {
                    rateMst1 = 0;
                }
                else
                {
                    rateMst1 = HokenMstFutanRate;
                }

                int rateMst2 = 0;
                if (HokenKbn == 1 || HokenKbn == 2)
                {
                    // 未就学児:
                    if (IsPreSchool())
                    {
                        rateMst2 = 20;
                    }
                    //後期:
                    else if (isHoken39)
                    {
                        if (KogakuKbn == 3
                            || (new List<int>() { 26, 27, 28 }.Contains(KogakuKbn)))
                        {
                            return 30;
                        }
                        else
                        {
                            rateMst2 = HokenMstFutanRate;  // 10%
                        }
                    }
                    // 高齢:
                    else if (IsElder())
                    {
                        if (KogakuKbn == 3
                            || (new List<int>() { 26, 27, 28 }.Contains(KogakuKbn)))
                        {
                            return 30;
                            //rateMst2 = HokenInf.HokenMasterModel.FutanRate;  // 30%
                        }
                        else if (Age >= 75 && SinDate >= KaiseiDate.d20140501)
                        {
                            rateMst2 = 20;
                        }
                        else if (CIUtil.Is70Zenki_20per(Birthday, SinDate))
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

        private bool IsPreSchool()
        {
            return !CIUtil.IsStudent(Birthday, SinDate);
        }

        private bool IsElder()
        {
            return CIUtil.AgeChk(Birthday, SinDate, 70);
        }

        private int Age
        {
            get => CIUtil.SDateToAge(Birthday, SinDate);
        }

        public bool IsEmptyKohi1
        {
            get => (Kohi1Id == 0 || Kohi1 == null);
        }

        public bool IsEmptyKohi2
        {
            get => (Kohi2Id == 0 || Kohi2 == null);
        }

        public bool IsEmptyKohi3
        {
            get => (Kohi3Id == 0 || Kohi3 == null);
        }

        public bool IsEmptyKohi4
        {
            get => (Kohi4Id == 0 || Kohi4 == null);
        }

    }
}