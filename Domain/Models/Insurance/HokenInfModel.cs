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
        public HokenInfModel(int hpId, long ptId, int hokenId, long seqNo, int hokenNo, int hokenEdaNo, int hokenKbn, string hokensyaNo, string kigo, string bango, string edaNo, int honkeKbn, int startDate, int endDate, int sikakuDate, int kofuDate, int confirmDate, int kogakuKbn, int tasukaiYm, int tokureiYm1, int tokureiYm2, int genmenKbn, int genmenRate, int genmenGaku, int syokumuKbn, int keizokuKbn, string tokki1, string tokki2, string tokki3, string tokki4, string tokki5, string rousaiKofuNo, string nenkinBango, string rousaiRoudouCd, string kenkoKanriBango, int rousaiSaigaiKbn, string rousaiKantokuCd, int rousaiSyobyoDate, int ryoyoStartDate, int ryoyoEndDate, string rousaiSyobyoCd, string rousaiJigyosyoName, string rousaiPrefName, string rousaiCityName, int rousaiReceCount, int rousaiTenkiSinkei, int rousaiTenkiTenki, int rousaiTenkiEndDate, string hokenMstHoubetu, int hokenMstFutanRate, int hokenMstFutanKbn, int sinDate, string jibaiHokenName, string jibaiHokenTanto, string jibaiHokenTel, int jibaiJyusyouDate, int isHaveHokenMst, int hokenMstSubNumber, string houbetu, List<ConfirmDateModel> confirmDateList)
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
            ConfirmDateList = confirmDateList;
        }

        public HokenInfModel(int hokenId, int startDate, int endDate)
        {
            HokenId = hokenId;
            StartDate = startDate;
            EndDate = endDate;
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

        public string NenkinBango { get; private set; } = string.Empty;

        public string RousaiRoudouCd { get; private set; } = string.Empty;

        public string KenkoKanriBango { get; private set; } = string.Empty;

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

        public int RousaiTenkiSinkei { get; private set; }

        public int RousaiTenkiTenki { get; private set; }

        public int RousaiTenkiEndDate { get; private set; }

        public string HokenMstHoubetu { get; private set; } = string.Empty;

        public int HokenMstFutanRate { get; private set; }

        public int HokenMstFutanKbn { get; private set; }

        public int SinDate { get; private set; }

        public string JibaiHokenName { get; private set; } = string.Empty;

        public string JibaiHokenTanto { get; private set; } = string.Empty;

        public string JibaiHokenTel { get; private set; } = string.Empty;

        public int JibaiJyusyouDate { get; private set; }

        public int IsHaveHokenMst { get; private set; }

        public int HokenMstSubNumber { get; private set; }

        public string Houbetu { get; private set; } = string.Empty;

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
