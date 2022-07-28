using Domain.Models.Insurance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.InsuranceInfor
{
    public class InsuranceModel
    {
        public InsuranceModel(int hpId, long ptId, int hokenId, long seqNo, int hokenNo, int hokenEdaNo, int hokenSbtCd, int hokenPid, int hokenKbn, int kohi1Id, int kohi2Id, int kohi3Id, int kohi4Id, string hokenName, string hokensyaNo, string kigo, string bango, string edaNo, int honkeKbn, int startDate, int endDate, int sikakuDate, int kofuDate, int confirmDate, KohiInfModel? kohi1, KohiInfModel? kohi2, KohiInfModel? kohi3, KohiInfModel? kohi4, int kogakuKbn, int tasukaiYm, int tokureiYm1, int tokureiYm2, int genmenKbn, int genmenRate, int genmenGaku, int syokumuKbn, int keizokuKbn, string tokki1, string tokki2, string tokki3, string tokki4, string tokki5, string rousaiKofuNo, string nenkinBango, string rousaiRoudouCd, string kenkoKanriBango, int rousaiSaigaiKbn, string rousaiKantokuCd, int rousaiSyobyoDate, int ryoyoStartDate, int ryoyoEndDate, string rousaiSyobyoCd, string rousaiJigyosyoName, string rousaiPrefName, string rousaiCityName, int rousaiReceCount, int rousaiTenkiSinkei, int rousaiTenkiTenki, int rousaiTenkiEndDate)
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
            HokenName = hokenName;
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
        public string HokenName { get; set; }
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
        public KohiInfModel? Kohi1 { get; private set; }
        public KohiInfModel? Kohi2 { get; private set; }
        public KohiInfModel? Kohi3 { get; private set; }
        public KohiInfModel? Kohi4 { get; private set; }
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
        //detail 2 3 
        public int RousaiTenkiSinkei { get; set; }
        public int RousaiTenkiTenki { get; set; }
        public int RousaiTenkiEndDate { get; set; }
    }
}