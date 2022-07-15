using Domain.CommonObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.InsuranceInfor
{
    public class InsuranceInforModel
    {
        public InsuranceInforModel(HpId hpId, PtId ptId, int hokenId, long seqNo, int hokenNo, string edaNo, int hokenEdaNo, string hokensyaNo, string kigo, string bango, int honkeKbn, int hokenKbn, string houbetu, string hokensyaName, string hokensyaPost, string hokensyaAddress, string hokensyaTel, int keizokuKbn, int sikakuDate, int kofuDate, int startDate, int endDate, int rate, int gendogaku, int kogakuKbn, int kogakuType, int tokureiYm1, int tokureiYm2, int tasukaiYm, int syokumuKbn, int genmenKbn, int genmenRate, int genmenGaku, string tokki1, string tokki2, string tokki3, string tokki4, string tokki5, string rousaiKofuNo, int rousaiSaigaiKbn, string rousaiJigyosyoName, string rousaiPrefName, string rousaiCityName, int rousaiSyobyoDate, string rousaiSyobyoCd, string rousaiRoudouCd, string rousaiKantokuCd, int rousaiReceCount, int ryoyoStartDate, int ryoyoEndDate, string jibaiHokenName, string jibaiHokenTanto, string jibaiHokenTel, int jibaiJyusyouDate)
        {
            HpId = hpId;
            PtId = ptId;
            HokenId = hokenId;
            SeqNo = seqNo;
            HokenNo = hokenNo;
            EdaNo = edaNo;
            HokenEdaNo = hokenEdaNo;
            HokensyaNo = hokensyaNo;
            Kigo = kigo;
            Bango = bango;
            HonkeKbn = honkeKbn;
            HokenKbn = hokenKbn;
            Houbetu = houbetu;
            HokensyaName = hokensyaName;
            HokensyaPost = hokensyaPost;
            HokensyaAddress = hokensyaAddress;
            HokensyaTel = hokensyaTel;
            KeizokuKbn = keizokuKbn;
            SikakuDate = sikakuDate;
            KofuDate = kofuDate;
            StartDate = startDate;
            EndDate = endDate;
            Rate = rate;
            Gendogaku = gendogaku;
            KogakuKbn = kogakuKbn;
            KogakuType = kogakuType;
            TokureiYm1 = tokureiYm1;
            TokureiYm2 = tokureiYm2;
            TasukaiYm = tasukaiYm;
            SyokumuKbn = syokumuKbn;
            GenmenKbn = genmenKbn;
            GenmenRate = genmenRate;
            GenmenGaku = genmenGaku;
            Tokki1 = tokki1;
            Tokki2 = tokki2;
            Tokki3 = tokki3;
            Tokki4 = tokki4;
            Tokki5 = tokki5;
            RousaiKofuNo = rousaiKofuNo;
            RousaiSaigaiKbn = rousaiSaigaiKbn;
            RousaiJigyosyoName = rousaiJigyosyoName;
            RousaiPrefName = rousaiPrefName;
            RousaiCityName = rousaiCityName;
            RousaiSyobyoDate = rousaiSyobyoDate;
            RousaiSyobyoCd = rousaiSyobyoCd;
            RousaiRoudouCd = rousaiRoudouCd;
            RousaiKantokuCd = rousaiKantokuCd;
            RousaiReceCount = rousaiReceCount;
            RyoyoStartDate = ryoyoStartDate;
            RyoyoEndDate = ryoyoEndDate;
            JibaiHokenName = jibaiHokenName;
            JibaiHokenTanto = jibaiHokenTanto;
            JibaiHokenTel = jibaiHokenTel;
            JibaiJyusyouDate = jibaiJyusyouDate;
        }

        public HpId HpId { get; set; }
        public PtId PtId { get; set; }
        public int HokenId { get; set; }
        public long SeqNo { get; set; }
        public int HokenNo { get; set; }
        public string EdaNo { get; set; }
        public int HokenEdaNo { get; set; }
        public string HokensyaNo { get; set; }
        public string Kigo { get; set; }
        public string Bango { get; set; }
        public int HonkeKbn { get; set; }
        public int HokenKbn { get; set; }
        public string Houbetu { get; set; }
        public string HokensyaName { get; set; }
        public string HokensyaPost { get; set; }
        public string HokensyaAddress { get; set; }
        public string HokensyaTel { get; set; }
        public int KeizokuKbn { get; set; }
        public int SikakuDate { get; set; }
        public int KofuDate { get; set; }
        public int StartDate { get; set; }
        public int EndDate { get; set; }
        public int Rate { get; set; }
        public int Gendogaku { get; set; }
        public int KogakuKbn { get; set; }
        public int KogakuType { get; set; }
        public int TokureiYm1 { get; set; }
        public int TokureiYm2 { get; set; }
        public int TasukaiYm { get; set; }
        public int SyokumuKbn { get; set; }
        public int GenmenKbn { get; set; }
        public int GenmenRate { get; set; }
        public int GenmenGaku { get; set; }
        public string Tokki1 { get; set; }
        public string Tokki2 { get; set; }
        public string Tokki3 { get; set; }
        public string Tokki4 { get; set; }
        public string Tokki5 { get; set; }
        public string RousaiKofuNo { get; set; }
        public int RousaiSaigaiKbn { get; set; }
        public string RousaiJigyosyoName { get; set; }
        public string RousaiPrefName { get; set; }
        public string RousaiCityName { get; set; }
        public int RousaiSyobyoDate { get; set; }
        public string RousaiSyobyoCd { get; set; }
        public string RousaiRoudouCd { get; set; }
        public string RousaiKantokuCd { get; set; }
        public int RousaiReceCount { get; set; }
        public int RyoyoStartDate { get; set; }
        public int RyoyoEndDate { get; set; }
        public string JibaiHokenName { get; set; }
        public string JibaiHokenTanto { get; set; }
        public string JibaiHokenTel { get; set; }
        public int JibaiJyusyouDate { get; set; }


    }
}
