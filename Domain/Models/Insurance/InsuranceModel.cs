using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.InsuranceInfor
{
    public class InsuranceModel
    {
        public InsuranceModel(int hpId, long ptId, int hokenId, long seqNo, int hokenNo, int hokenEdaNo, int hokenSbtCd, int hokenPid, int hokenKbn, int kohi1Id, int kohi2Id, int kohi3Id, int kohi4Id, string? hokenName, string? hokensyaNo, string? kigo, string? bango, string? edaNo, int honkeKbn, int startDate, int endDate, int sikakuDate, int kofuDate, int confirmDate, string kohi1FutansyaNo, string kohi1JyukyusyaNo, int kohi1HokenId, int kohi1StartDate, int kohi1EndDate, int kohi1ConfirmDate, int kohi1InfRate, int kohi1InfGendoGaku, int kohi1InfSikakuDate, int kohi1InfKofuDate, string kohi1InfTokusyuNo, string kohi2FutansyaNo, string kohi2JyukyusyaNo, int kohi2HokenId, int kohi2StartDate, int kohi2EndDate, int kohi2ConfirmDate, int kohi2InfRate, int kohi2InfGendoGaku, int kohi2InfSikakuDate, int kohi2InfKofuDate, string kohi2InfTokusyuNo, string kohi3FutansyaNo, string kohi3JyukyusyaNo, int kohi3HokenId, int kohi3StartDate, int kohi3EndDate, int kohi3ConfirmDate, int kohi3InfRate, int kohi3InfGendoGaku, int kohi3InfSikakuDate, int kohi3InfKofuDate, string kohi3InfTokusyuNo, string kohi4FutansyaNo, string kohi4JyukyusyaNo, int kohi4HokenId, int kohi4StartDate, int kohi4EndDate, int kohi4ConfirmDate, int kohi4InfRate, int kohi4InfGendoGaku, int kohi4InfSikakuDate, int kohi4InfKofuDate, string kohi4InfTokusyuNo, int kogakuKbn, int tasukaiYm, int tokureiYm1, int tokureiYm2, int genmenKbn, int genmenRate, int genmenGaku, int syokumuKbn, int keizokuKbn, string? tokki1, string? tokki2, string? tokki3, string? tokki4, string? tokki5, string? rousaiKofuNo, string nenkinBango, string? rousaiRoudouCd, string kenkoKanriBango, int rousaiSaigaiKbn, string? rousaiKantokuCd, int rousaiSyobyoDate, int ryoyoStartDate, int ryoyoEndDate, string? rousaiSyobyoCd, string? rousaiJigyosyoName, string? rousaiPrefName, string? rousaiCityName, int rousaiReceCount, int rousaiTenkiSinkei, int rousaiTenkiTenki, int rousaiTenkiEndDate)
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
            Kohi1FutansyaNo = kohi1FutansyaNo;
            Kohi1JyukyusyaNo = kohi1JyukyusyaNo;
            Kohi1HokenId = kohi1HokenId;
            Kohi1StartDate = kohi1StartDate;
            Kohi1EndDate = kohi1EndDate;
            Kohi1ConfirmDate = kohi1ConfirmDate;
            Kohi1InfRate = kohi1InfRate;
            Kohi1InfGendoGaku = kohi1InfGendoGaku;
            Kohi1InfSikakuDate = kohi1InfSikakuDate;
            Kohi1InfKofuDate = kohi1InfKofuDate;
            Kohi1InfTokusyuNo = kohi1InfTokusyuNo;
            Kohi2FutansyaNo = kohi2FutansyaNo;
            Kohi2JyukyusyaNo = kohi2JyukyusyaNo;
            Kohi2HokenId = kohi2HokenId;
            Kohi2StartDate = kohi2StartDate;
            Kohi2EndDate = kohi2EndDate;
            Kohi2ConfirmDate = kohi2ConfirmDate;
            Kohi2InfRate = kohi2InfRate;
            Kohi2InfGendoGaku = kohi2InfGendoGaku;
            Kohi2InfSikakuDate = kohi2InfSikakuDate;
            Kohi2InfKofuDate = kohi2InfKofuDate;
            Kohi2InfTokusyuNo = kohi2InfTokusyuNo;
            Kohi3FutansyaNo = kohi3FutansyaNo;
            Kohi3JyukyusyaNo = kohi3JyukyusyaNo;
            Kohi3HokenId = kohi3HokenId;
            Kohi3StartDate = kohi3StartDate;
            Kohi3EndDate = kohi3EndDate;
            Kohi3ConfirmDate = kohi3ConfirmDate;
            Kohi3InfRate = kohi3InfRate;
            Kohi3InfGendoGaku = kohi3InfGendoGaku;
            Kohi3InfSikakuDate = kohi3InfSikakuDate;
            Kohi3InfKofuDate = kohi3InfKofuDate;
            Kohi3InfTokusyuNo = kohi3InfTokusyuNo;
            Kohi4FutansyaNo = kohi4FutansyaNo;
            Kohi4JyukyusyaNo = kohi4JyukyusyaNo;
            Kohi4HokenId = kohi4HokenId;
            Kohi4StartDate = kohi4StartDate;
            Kohi4EndDate = kohi4EndDate;
            Kohi4ConfirmDate = kohi4ConfirmDate;
            Kohi4InfRate = kohi4InfRate;
            Kohi4InfGendoGaku = kohi4InfGendoGaku;
            Kohi4InfSikakuDate = kohi4InfSikakuDate;
            Kohi4InfKofuDate = kohi4InfKofuDate;
            Kohi4InfTokusyuNo = kohi4InfTokusyuNo;
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

        public int HpId { get; set; }
        public long PtId { get; set; }
        public int HokenId { get; set; }
        public long SeqNo { get; set; }
        public int HokenNo { get; set; }
        public int HokenEdaNo { get; set; }
        public int HokenSbtCd { get; set; }
        public int HokenPid { get; set; }
        public int HokenKbn { get; set; }
        public int Kohi1Id { get; set; }
        public int Kohi2Id { get; set; }
        public int Kohi3Id { get; set; }
        public int Kohi4Id { get; set; }
        public string? HokenName { get; set; }
        public string? HokensyaNo { get; set; }
        public string? Kigo { get; set; }
        public string? Bango { get; set; }
        public string? EdaNo { get; set; }
        public int HonkeKbn { get; set; }
        public int StartDate { get; set; }
        public int EndDate { get; set; }
        public int SikakuDate { get; set; }
        public int KofuDate { get; set; }
        public int ConfirmDate { get; set; }

        //KOHI1
        public string Kohi1FutansyaNo { get; set; }
        public string Kohi1JyukyusyaNo { get; set; }
        public int Kohi1HokenId { get; set; }
        public int Kohi1StartDate { get; set; }
        public int Kohi1EndDate { get; set; }
        public int Kohi1ConfirmDate { get; set; }

        public int Kohi1InfRate { get; set; }
        public int Kohi1InfGendoGaku { get; set; }
        public int Kohi1InfSikakuDate { get; set; }
        public int Kohi1InfKofuDate { get; set; }
        public string Kohi1InfTokusyuNo { get; set; }
        //KOHI2
        public string Kohi2FutansyaNo { get; set; }
        public string Kohi2JyukyusyaNo { get; set; }
        public int Kohi2HokenId { get; set; }
        public int Kohi2StartDate { get; set; }
        public int Kohi2EndDate { get; set; }
        public int Kohi2ConfirmDate { get; set; }

        public int Kohi2InfRate { get; set; }
        public int Kohi2InfGendoGaku { get; set; }
        public int Kohi2InfSikakuDate { get; set; }
        public int Kohi2InfKofuDate { get; set; }
        public string Kohi2InfTokusyuNo { get; set; }
        //KOHI3
        public string Kohi3FutansyaNo { get; set; }
        public string Kohi3JyukyusyaNo { get; set; }
        public int Kohi3HokenId { get; set; }
        public int Kohi3StartDate { get; set; }
        public int Kohi3EndDate { get; set; }
        public int Kohi3ConfirmDate { get; set; }

        public int Kohi3InfRate { get; set; }
        public int Kohi3InfGendoGaku { get; set; }
        public int Kohi3InfSikakuDate { get; set; }
        public int Kohi3InfKofuDate { get; set; }
        public string Kohi3InfTokusyuNo { get; set; }
        //KOHI4
        public string Kohi4FutansyaNo { get; set; }
        public string Kohi4JyukyusyaNo { get; set; }
        public int Kohi4HokenId { get; set; }
        public int Kohi4StartDate { get; set; }
        public int Kohi4EndDate { get; set; }
        public int Kohi4ConfirmDate { get; set; }

        public int Kohi4InfRate { get; set; }
        public int Kohi4InfGendoGaku { get; set; }
        public int Kohi4InfSikakuDate { get; set; }
        public int Kohi4InfKofuDate { get; set; }
        public string Kohi4InfTokusyuNo { get; set; }
        // detail 1
        public int KogakuKbn { get; set; }
        public int TasukaiYm { get; set; }
        public int TokureiYm1 { get; set; }
        public int TokureiYm2 { get; set; }
        public int GenmenKbn { get; set; }
        public int GenmenRate { get; set; }
        public int GenmenGaku { get; set; }
        public int SyokumuKbn { get; set; }
        public int KeizokuKbn { get; set; }
        public string? Tokki1 { get; set; }
        public string? Tokki2 { get; set; }
        public string? Tokki3 { get; set; }
        public string? Tokki4 { get; set; }
        public string? Tokki5 { get; set; }
        //2
        public string? RousaiKofuNo { get; set; }
        public string NenkinBango { get; set; }
        public string? RousaiRoudouCd { get; set; }
        public string KenkoKanriBango { get; set; }
        public int RousaiSaigaiKbn { get; set; }
        public string? RousaiKantokuCd { get; set; }
        public int RousaiSyobyoDate { get; set; }
        public int RyoyoStartDate { get; set; }
        public int RyoyoEndDate { get; set; }
        public string? RousaiSyobyoCd { get; set; }
        public string? RousaiJigyosyoName { get; set; }
        public string? RousaiPrefName { get; set; }
        public string? RousaiCityName { get; set; }
        public int RousaiReceCount { get; set; }

        //detail 2 3 
        public int RousaiTenkiSinkei { get; set; }
        public int RousaiTenkiTenki { get; set; }
        public int RousaiTenkiEndDate { get; set; }
    }
}