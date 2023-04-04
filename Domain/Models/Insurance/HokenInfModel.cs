﻿using Domain.Models.InsuranceMst;
using Helper.Constants;
using System.Text.Json.Serialization;

namespace Domain.Models.Insurance
{
    public class HokenInfModel
    {
        [JsonConstructor]
        public HokenInfModel(int hpId, long ptId, int hokenId, long seqNo, int hokenNo, int hokenEdaNo, int hokenKbn, string hokensyaNo, string kigo, string bango, string edaNo, int honkeKbn, int startDate, int endDate, int sikakuDate, int kofuDate, int confirmDate, int kogakuKbn, int tasukaiYm, int tokureiYm1, int tokureiYm2, int genmenKbn, int genmenRate, int genmenGaku, int syokumuKbn, int keizokuKbn, string tokki1, string tokki2, string tokki3, string tokki4, string tokki5, string rousaiKofuNo, string rousaiRoudouCd, int rousaiSaigaiKbn, string rousaiKantokuCd, int rousaiSyobyoDate, int ryoyoStartDate, int ryoyoEndDate, string rousaiSyobyoCd, string rousaiJigyosyoName, string rousaiPrefName, string rousaiCityName, int rousaiReceCount, string hokensyaName, string hokensyaAddress, string hokensyaTel, int sinDate, string jibaiHokenName, string jibaiHokenTanto, string jibaiHokenTel, int jibaiJyusyouDate, string houbetu, List<ConfirmDateModel> confirmDateList, List<RousaiTenkiModel> listRousaiTenki, bool isReceKisaiOrNoHoken, int isDeleted, HokenMstModel hokenMst, HokensyaMstModel hokensyaMst, bool isAddNew, bool isAddHokenCheck)
        {
            HpId = hpId;
            PtId = ptId;
            HokenId = hokenId;
            SeqNo = seqNo;
            HokenNo = hokenNo;
            HokenEdaNo = hokenEdaNo;
            HokenKbn = hokenKbn;
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
            RousaiRoudouCd = rousaiRoudouCd;
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
            HokensyaName = hokensyaName;
            HokensyaAddress = hokensyaAddress;
            HokensyaTel = hokensyaTel;
            SinDate = sinDate;
            JibaiHokenName = jibaiHokenName;
            JibaiHokenTanto = jibaiHokenTanto;
            JibaiHokenTel = jibaiHokenTel;
            JibaiJyusyouDate = jibaiJyusyouDate;
            Houbetu = houbetu;
            ConfirmDateList = confirmDateList;
            ListRousaiTenki = listRousaiTenki;
            IsReceKisaiOrNoHoken = isReceKisaiOrNoHoken;
            IsDeleted = isDeleted;
            HokenMst = hokenMst;
            IsAddNew = isAddNew;
            IsAddHokenCheck = isAddHokenCheck;
            HokensyaMst = hokensyaMst;
        }

        public HokenInfModel(int hokenId, int startDate, int endDate)
        {
            HokenId = hokenId;
            StartDate = startDate;
            EndDate = endDate;
            HokenMst = new();
            HokensyaMst = new();
        }

        public HokenInfModel(int hokenId, long ptId, int hpId, int startDate, int endDate)
        {
            HokenId = hokenId;
            HpId = hpId;
            PtId = ptId;
            StartDate = startDate;
            EndDate = endDate;
            HokenMst = new();
            HokensyaMst = new();
        }
        public HokenInfModel()
        {
            HokenMst = new();
            HokensyaMst = new();
        }

        public HokenInfModel(int hpId, long ptId, int hokenId, int hokenKbn, string houbetu, int startDate, int endDate, int sinDate, HokenMstModel hokenMst, List<ConfirmDateModel> confirmDateModels)
        {
            HpId = hpId;
            PtId = ptId;
            HokenId = hokenId;
            HokenKbn = hokenKbn;
            Houbetu = houbetu;
            StartDate = startDate;
            EndDate = endDate;
            SinDate = sinDate;
            HokenMst = hokenMst;
            ConfirmDateList = confirmDateModels;
            HokensyaMst = new();
        }

        public List<ConfirmDateModel> ConfirmDateList { get; private set; } = new List<ConfirmDateModel>();

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int HokenId { get; private set; }

        public long SeqNo { get; private set; }

        public int HokenNo { get; private set; }

        public int HokenEdaNo { get; private set; }

        public int HokenKbn { get; private set; }

        public string HokensyaNo { get; private set; } = string.Empty;

        public string Kigo { get; private set; } = string.Empty;

        public string Bango { get; private set; } = string.Empty;

        public string EdaNo { get; private set; } = string.Empty;

        public int HonkeKbn { get; private set; }

        public int StartDate { get; private set; }

        public int EndDate { get; private set; }

        public int SikakuDate { get; private set; }

        public int KofuDate { get; private set; }

        public int ConfirmDate { get; private set; }

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

        public string Tokki1 { get; private set; } = string.Empty;

        public string Tokki2 { get; private set; } = string.Empty;

        public string Tokki3 { get; private set; } = string.Empty;

        public string Tokki4 { get; private set; } = string.Empty;

        public string Tokki5 { get; private set; } = string.Empty;

        //2
        public string RousaiKofuNo { get; private set; } = string.Empty;


        public string RousaiRoudouCd { get; private set; } = string.Empty;


        public int RousaiSaigaiKbn { get; private set; }

        public string RousaiKantokuCd { get; private set; } = string.Empty;

        public int RousaiSyobyoDate { get; private set; }

        public int RyoyoStartDate { get; private set; }

        public int RyoyoEndDate { get; private set; }

        public string RousaiSyobyoCd { get; private set; } = string.Empty;

        public string RousaiJigyosyoName { get; private set; } = string.Empty;

        public string RousaiPrefName { get; private set; } = string.Empty;

        public string RousaiCityName { get; private set; } = string.Empty;

        public int RousaiReceCount { get; private set; }

        public int SinDate { get; private set; }

        public string JibaiHokenName { get; private set; } = string.Empty;

        public string JibaiHokenTanto { get; private set; } = string.Empty;

        public string JibaiHokenTel { get; private set; } = string.Empty;

        public int JibaiJyusyouDate { get; private set; }

        public string Houbetu { get; private set; } = string.Empty;

        public string HokensyaName { get; private set; } = string.Empty;

        public string HokensyaAddress { get; private set; } = string.Empty;

        public string HokensyaTel { get; private set; } = string.Empty;

        public List<RousaiTenkiModel> ListRousaiTenki { get; private set; } = new List<RousaiTenkiModel>();

        public bool IsReceKisaiOrNoHoken { get; private set; }

        public int IsDeleted { get; private set; }

        public HokenMstModel HokenMst { get; private set; }

        public bool IsHaveHokenMst { get => HokenMst != null; }

        public string HokenMstHoubetu => HokenMst != null ? HokenMst.Houbetu : string.Empty;

        public int HokenMstFutanRate => HokenMst != null ? HokenMst.FutanRate : 0;

        public int HokenMstFutanKbn => HokenMst != null ? HokenMst.FutanKbn : 0;

        public int HokenMstSbtKbn => HokenMst != null ? HokenMst.HokenSbtKbn : 0;

        public int HokenMstStartDate => HokenMst != null ? HokenMst.StartDate : 0;

        public int HokenMstEndDate => HokenMst != null ? HokenMst.EndDate : 0;

        public string HokenMstDisplayTextMaster => HokenMst != null ? HokenMst.DisplayTextMaster : string.Empty;

        public bool IsJihi
        {
            get
            {
                if (IsHaveHokenMst)
                {
                    return HokenMst.HokenSbtKbn == 8;
                }
                return HokenKbn == 0 && (Houbetu == HokenConstant.HOUBETU_JIHI_108 || Houbetu == HokenConstant.HOUBETU_JIHI_109);
            }
        }

        public bool IsExpirated
        {
            get
            {
                return !(StartDate <= SinDate && EndDate >= SinDate);
            }
        }

        public bool IsAddNew { get; private set; }

        public bool IsShaho
        {
            // not nashi
            get => HokenKbn == 1 && Houbetu != HokenConstant.HOUBETU_NASHI;
        }

        public bool IsKokuho
        {
            get => HokenKbn == 2;
        }

        public bool IsShahoOrKokuho => IsShaho || IsKokuho;

        public bool IsNoHoken
        {
            get
            {
                if (HokenMst != null)
                {
                    return HokenMst.HokenSbtKbn == 0;
                }
                return HokenKbn == 1 && Houbetu == HokenConstant.HOUBETU_NASHI;
            }
        }

        public bool IsAddHokenCheck { get; private set; }

        public string RodoBango
        {
            get
            {
                if (!IsNotRodo)
                {
                    return RousaiKofuNo;
                }
                return string.Empty;
            }
        }

        public string NenkinBango
        {
            get
            {
                if (!IsNotNenkin)
                {
                    return RousaiKofuNo;
                }
                return string.Empty;
            }
        }

        public string KenkoKanriBango
        {
            get
            {
                if (!IsNotKenkoKanri)
                {
                    return RousaiKofuNo;
                }
                return string.Empty;
            }
        }

        public HokensyaMstModel HokensyaMst { get; private set; }

        public bool IsNotNenkin => HokenKbn != 12;

        public bool IsNotKenkoKanri => HokenKbn != 13;

        public bool IsNotRodo => HokenKbn != 11;

        public bool IsRousai => HokenKbn == 11 || HokenKbn == 12 || HokenKbn == 13;

        public bool IsJibai => HokenKbn == 14;

        public bool IsHoken => IsShaho || IsKokuho;

        public bool HasDateConfirmed
        {
            get
            {
                // Jihi
                if (IsJihi)
                {
                    return true;
                }

                // nashi
                if (Houbetu == HokenConstant.HOUBETU_NASHI)
                {
                    return true;
                }

                return false;
            }
        }

        public int LastDateConfirmed
        {
            get
            {
                if (!ConfirmDateList.Any()) return 0;

                return ConfirmDateList.OrderByDescending(item => item.ConfirmDate).First().ConfirmDate;
            }
        }

        public string HokenSentaku
        {
            get
            {
                string result = string.Empty;
                result += HokenId.ToString().PadLeft(2, '0');
                result += " ";
                if (string.IsNullOrEmpty(HokensyaNo) && HokenKbn < 10)
                {
                    // jihi
                    if (HokenKbn == 0 || (HokenKbn == 1 && HokenMst != null && HokenMst.HokenSbtKbn == 0))
                    {
                        result += HokenMst?.HokenSName;
                    }
                    if (IsExpirated)
                    {
                        result = "×" + result;
                    }
                    return result;
                }
                switch (HokenKbn)
                {
                    case 1:
                        result += "社保";
                        break;
                    case 2:
                        if (HokensyaNo.Length == 8 &&
                            HokensyaNo.StartsWith("39"))
                        {
                            result += "後期";
                        }
                        else if (HokensyaNo.Length == 8 &&
                            HokensyaNo.StartsWith("67"))
                        {
                            result += "退職";
                        }
                        else
                        {
                            result += "国保";
                        }
                        break;
                    case 11:
                        result += "労災（短期給付）";
                        break;
                    case 12:
                        result += "労災（傷病年金）";
                        break;
                    case 13:
                        result += "労災（アフターケア）";
                        break;
                    case 14:
                        result += "自賠責";
                        break;
                }
                if (HonkeKbn != 0)
                {
                    result += "(";
                    if (HonkeKbn == 1)
                    {
                        result += "本人";
                    }
                    else
                    {
                        result += "家族";
                    }
                    result += ")";
                }

                if (!string.IsNullOrEmpty(HokensyaNo))
                {
                    result += " ";
                    result += HokensyaNo;
                }

                if (IsExpirated)
                {
                    result = "×" + result;
                }
                return result;
            }
        }
    }
}
