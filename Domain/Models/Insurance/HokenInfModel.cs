using Domain.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Insurance
{
    public class HokenInfModel
    {
        public HokenInfModel(int hpId, long ptId, int hokenId, long seqNo, int hokenNo, int hokenEdaNo, int hokenKbn, string hokensyaNo, string kigo, string bango, string edaNo, int honkeKbn, int startDate, int endDate, int sikakuDate, int kofuDate, int confirmDate, int kogakuKbn, int tasukaiYm, int tokureiYm1, int tokureiYm2, int genmenKbn, int genmenRate, int genmenGaku, int syokumuKbn, int keizokuKbn, string tokki1, string tokki2, string tokki3, string tokki4, string tokki5, string rousaiKofuNo, string nenkinBango, string rousaiRoudouCd, string kenkoKanriBango, int rousaiSaigaiKbn, string rousaiKantokuCd, int rousaiSyobyoDate, int ryoyoStartDate, int ryoyoEndDate, string rousaiSyobyoCd, string rousaiJigyosyoName, string rousaiPrefName, string rousaiCityName, int rousaiReceCount, int rousaiTenkiSinkei, int rousaiTenkiTenki, int rousaiTenkiEndDate, string hokenMstHoubetu, int hokenMstFutanRate, int hokenMstFutanKbn, int sinDate, string jibaiHokenName, string jibaiHokenTanto, string jibaiHokenTel, int jibaiJyusyouDate, int isHaveHokenMst, int hokenMstSubNumber, string houbetu)
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
            HokenMstHoubetu = hokenMstHoubetu;
            HokenMstFutanRate = hokenMstFutanRate;
            HokenMstFutanKbn = hokenMstFutanKbn;
            SinDate = sinDate;
            JibaiHokenName = jibaiHokenName;
            JibaiHokenTanto = jibaiHokenTanto;
            JibaiHokenTel = jibaiHokenTel;
            JibaiJyusyouDate = jibaiJyusyouDate;
            IsHaveHokenMst = isHaveHokenMst;
            HokenMstSubNumber = hokenMstSubNumber;
            Houbetu = houbetu;
        }

        public HokenInfModel()
        {
            HpId = 0;
            PtId = 0;
            HokenId = 0;
            SeqNo = 0;
            HokenNo = 0;
            HokenEdaNo = 0;
            HokenKbn = 0;
            HokensyaNo = string.Empty;
            Kigo = string.Empty;
            Bango = string.Empty;
            EdaNo = string.Empty;
            HonkeKbn = 0;
            StartDate = 0;
            EndDate = 0;
            SikakuDate = 0;
            KofuDate = 0;
            ConfirmDate = 0;
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
            HokenMstFutanKbn = 0;
            SinDate = 0;
            JibaiHokenName = string.Empty;
            JibaiHokenTanto = string.Empty;
            JibaiHokenTel = string.Empty;
            JibaiJyusyouDate = 0;
            IsHaveHokenMst = 0;
            HokenMstSubNumber = 0;
            Houbetu = string.Empty;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int HokenId { get; private set; }

        public long SeqNo { get; private set; }

        public int HokenNo { get; private set; }

        public int HokenEdaNo { get; private set; }

        public int HokenKbn { get; private set; }

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

        public int HokenMstFutanKbn { get; private set; }

        public int SinDate { get; private set; }

        public string JibaiHokenName { get; private set; }

        public string JibaiHokenTanto { get; private set; }

        public string JibaiHokenTel { get; private set; }

        public int JibaiJyusyouDate { get; private set; }

        public int IsHaveHokenMst { get; private set; }

        public int HokenMstSubNumber { get; private set; }

        public string Houbetu { get; private set; }

        public bool IsJihi
        {
            get
            {
                if (IsHaveHokenMst != 0)
                {
                    return HokenMstSubNumber == 8;
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

    }
}
