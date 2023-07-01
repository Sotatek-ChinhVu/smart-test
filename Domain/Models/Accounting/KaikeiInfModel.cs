﻿using Helper.Extension;

namespace Domain.Models.Accounting
{
    public class KaikeiInfModel
    {
        public KaikeiInfModel(int hpId, long ptId, int sinDate, long raiinNo, int hokenId, int kohi1Id, int kohi2Id, int kohi3Id, int kohi4Id, int hokenKbn, int hokenSbtCd, string receSbt, string houbetu, string kohi1Houbetu, string kohi2Houbetu, string kohi3Houbetu, string kohi4Houbetu, int honkeKbn, int hokenRate, int ptRate, int dispRate, int tensu, int totalIryohi, int ptFutan, int jihiFutan, int jihiTax, int jihiOuttax, int jihiFutanTaxfree, int jihiFutanTaxNr, int jihiFutanTaxGen, int jihiFutanOuttaxNr, int jihiFutanOuttaxGen, int jihiTaxNr, int jihiTaxGen, int jihiOuttaxNr, int jihiOuttaxGen, int adjustFutan, int adjustRound, int totalPtFutan, int adjustFutanVal, int adjustFutanRange, int adjustRateVal, int adjustRateRange)
        {
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
            RaiinNo = raiinNo;
            HokenId = hokenId;
            Kohi1Id = kohi1Id;
            Kohi2Id = kohi2Id;
            Kohi3Id = kohi3Id;
            Kohi4Id = kohi4Id;
            HokenKbn = hokenKbn;
            HokenSbtCd = hokenSbtCd;
            ReceSbt = receSbt;
            Houbetu = houbetu;
            Kohi1Houbetu = kohi1Houbetu;
            Kohi2Houbetu = kohi2Houbetu;
            Kohi3Houbetu = kohi3Houbetu;
            Kohi4Houbetu = kohi4Houbetu;
            HonkeKbn = honkeKbn;
            HokenRate = hokenRate;
            PtRate = ptRate;
            DispRate = dispRate;
            Tensu = tensu;
            TotalIryohi = totalIryohi;
            PtFutan = ptFutan;
            JihiFutan = jihiFutan;
            JihiTax = jihiTax;
            JihiOuttax = jihiOuttax;
            JihiFutanTaxfree = jihiFutanTaxfree;
            JihiFutanTaxNr = jihiFutanTaxNr;
            JihiFutanTaxGen = jihiFutanTaxGen;
            JihiFutanOuttaxNr = jihiFutanOuttaxNr;
            JihiFutanOuttaxGen = jihiFutanOuttaxGen;
            JihiTaxNr = jihiTaxNr;
            JihiTaxGen = jihiTaxGen;
            JihiOuttaxNr = jihiOuttaxNr;
            JihiOuttaxGen = jihiOuttaxGen;
            AdjustFutan = adjustFutan;
            AdjustRound = adjustRound;
            TotalPtFutan = totalPtFutan;
            AdjustFutanVal = adjustFutanVal;
            AdjustFutanRange = adjustFutanRange;
            AdjustRateVal = adjustRateVal;
            AdjustRateRange = adjustRateRange;
            Kohi1Priority = string.Empty;
            Kohi2Priority = string.Empty;
            Kohi3Priority = string.Empty;
            Kohi4Priority = string.Empty;
        }

        public KaikeiInfModel(int hpId, long ptId, int sinDate, long raiinNo, int hokenId, int kohi1Id, int kohi2Id, int kohi3Id, int kohi4Id, int hokenKbn, int hokenSbtCd, string receSbt, string houbetu, string kohi1Houbetu, string kohi2Houbetu, string kohi3Houbetu, string kohi4Houbetu, int honkeKbn, int hokenRate, int ptRate, int dispRate, int tensu, int totalIryohi, int ptFutan, int jihiFutan, int jihiTax, int jihiOuttax, int jihiFutanTaxfree, int jihiFutanTaxNr, int jihiFutanTaxGen, int jihiFutanOuttaxNr, int jihiFutanOuttaxGen, int jihiTaxNr, int jihiTaxGen, int jihiOuttaxNr, int jihiOuttaxGen, int adjustFutan, int adjustRound, int totalPtFutan, int adjustFutanVal, int adjustFutanRange, int adjustRateVal, int adjustRateRange, string kohi1Priority, string kohi2Priority, string kohi3Priority, string kohi4Priority) : this(hpId, ptId, sinDate, raiinNo, hokenId, kohi1Id, kohi2Id, kohi3Id, kohi4Id, hokenKbn, hokenSbtCd, receSbt, houbetu, kohi1Houbetu, kohi2Houbetu, kohi3Houbetu, kohi4Houbetu, honkeKbn, hokenRate, ptRate, dispRate, tensu, totalIryohi, ptFutan, jihiFutan, jihiTax, jihiOuttax, jihiFutanTaxfree, jihiFutanTaxNr, jihiFutanTaxGen, jihiFutanOuttaxNr, jihiFutanOuttaxGen, jihiTaxNr, jihiTaxGen, jihiOuttaxNr, jihiOuttaxGen, adjustFutan, adjustRound, totalPtFutan, adjustFutanVal, adjustFutanRange, adjustRateVal, adjustRateRange)
        {
            Kohi1Priority = kohi1Priority;
            Kohi2Priority = kohi2Priority;
            Kohi3Priority = kohi3Priority;
            Kohi4Priority = kohi4Priority;
        }

        public KaikeiInfModel()
        {
            ReceSbt = string.Empty;
            Houbetu = string.Empty;
            Kohi1Houbetu = string.Empty;
            Kohi2Houbetu = string.Empty;
            Kohi3Houbetu = string.Empty;
            Kohi4Houbetu = string.Empty;
            Kohi1Priority = string.Empty;
            Kohi2Priority = string.Empty;
            Kohi3Priority = string.Empty;
            Kohi4Priority = string.Empty;
        }

        public KaikeiInfModel(long ptId, int sinDate)
        {
            PtId = ptId;
            SinDate = sinDate;
            ReceSbt = string.Empty;
            Houbetu = string.Empty;
            Kohi1Houbetu = string.Empty;
            Kohi2Houbetu = string.Empty;
            Kohi3Houbetu = string.Empty;
            Kohi4Houbetu = string.Empty;
            Kohi1Priority = string.Empty;
            Kohi2Priority = string.Empty;
            Kohi3Priority = string.Empty;
            Kohi4Priority = string.Empty;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int SinDate { get; private set; }

        public long RaiinNo { get; private set; }

        public int HokenId { get; private set; }

        public int Kohi1Id { get; private set; }

        public int Kohi2Id { get; private set; }

        public int Kohi3Id { get; private set; }

        public int Kohi4Id { get; private set; }

        public int HokenKbn { get; private set; }

        public int HokenSbtCd { get; private set; }

        public string ReceSbt { get; private set; }

        public string Houbetu { get; private set; }

        public string Kohi1Houbetu { get; private set; }

        public string Kohi2Houbetu { get; private set; }

        public string Kohi3Houbetu { get; private set; }

        public string Kohi4Houbetu { get; private set; }

        public int HonkeKbn { get; private set; }

        public int HokenRate { get; private set; }

        public int PtRate { get; private set; }

        public int DispRate { get; private set; }

        public int Tensu { get; private set; }

        public int TotalIryohi { get; private set; }

        public int PtFutan { get; private set; }

        public int JihiFutan { get; private set; }

        public int JihiTax { get; private set; }

        public int JihiOuttax { get; private set; }

        public int JihiFutanTaxfree { get; private set; }

        public int JihiFutanTaxNr { get; private set; }

        public int JihiFutanTaxGen { get; private set; }

        public int JihiFutanOuttaxNr { get; private set; }

        public int JihiFutanOuttaxGen { get; private set; }

        public int JihiTaxNr { get; private set; }

        public int JihiTaxGen { get; private set; }

        public int JihiOuttaxNr { get; private set; }

        public int JihiOuttaxGen { get; private set; }

        public int AdjustFutan { get; private set; }

        public int AdjustRound { get; private set; }

        public int TotalPtFutan { get; private set; }

        public int AdjustFutanVal { get; private set; }

        public int AdjustFutanRange { get; private set; }

        public int AdjustRateVal { get; private set; }

        public int AdjustRateRange { get; private set; }

        public string Kohi1Priority { get; private set; }

        public string Kohi2Priority { get; private set; }

        public string Kohi3Priority { get; private set; }

        public string Kohi4Priority { get; private set; }

        public string SinYmBinding => (SinDate / 100).AsString().Length == 6 ? (SinDate / 100).AsString().Insert(4, "/") : "";
    }
}
