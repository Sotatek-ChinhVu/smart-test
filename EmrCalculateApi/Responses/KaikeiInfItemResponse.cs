using KaikeiInfModelOfFutan = EmrCalculateApi.Futan.Models.KaikeiInfModel;

namespace Domain.Models.Futan
{
    public class KaikeiInfItemResponse
    {
        public KaikeiInfItemResponse(KaikeiInfModelOfFutan kaikeiInfModel)
        {
            HpId = kaikeiInfModel.HpId;
            PtId = kaikeiInfModel.PtId;
            SinDate = kaikeiInfModel.SinDate;
            RaiinNo = kaikeiInfModel.RaiinNo;
            HokenId = kaikeiInfModel.HokenId;
            Kohi1Id = kaikeiInfModel.Kohi1Id;
            Kohi2Id = kaikeiInfModel.Kohi2Id;
            Kohi3Id = kaikeiInfModel.Kohi3Id;
            Kohi4Id = kaikeiInfModel.Kohi4Id;
            HokenKbn = kaikeiInfModel.HokenKbn;
            HokenSbtCd = kaikeiInfModel.HokenSbtCd;
            ReceSbt = kaikeiInfModel.ReceSbt;
            Houbetu = kaikeiInfModel.Houbetu;
            Kohi1Houbetu = kaikeiInfModel.Kohi1Houbetu;
            Kohi2Houbetu = kaikeiInfModel.Kohi2Houbetu;
            Kohi3Houbetu = kaikeiInfModel.Kohi3Houbetu;
            Kohi4Houbetu = kaikeiInfModel.Kohi4Houbetu;
            HonkeKbn = kaikeiInfModel.HonkeKbn;
            HokenRate = kaikeiInfModel.HokenRate;
            PtRate = kaikeiInfModel.PtRate;
            DispRate = kaikeiInfModel.DispRate;
            Tensu = kaikeiInfModel.Tensu;
            TotalIryohi = kaikeiInfModel.TotalIryohi;
            PtFutan = kaikeiInfModel.PtFutan;
            JihiFutan = kaikeiInfModel.JihiFutan;
            JihiTax = kaikeiInfModel.JihiTax;
            JihiOuttax = kaikeiInfModel.JihiOuttax;
            JihiFutanTaxfree = kaikeiInfModel.JihiFutanTaxfree;
            JihiFutanTaxNr = kaikeiInfModel.JihiFutanTaxNr;
            JihiFutanTaxGen = kaikeiInfModel.JihiFutanTaxGen;
            JihiFutanOuttaxNr = kaikeiInfModel.JihiFutanOuttaxNr;
            JihiFutanOuttaxGen = kaikeiInfModel.JihiFutanOuttaxGen;
            JihiTaxNr = kaikeiInfModel.JihiTaxNr;
            JihiTaxGen = kaikeiInfModel.JihiTaxGen;
            JihiOuttaxNr = kaikeiInfModel.JihiOuttaxNr;
            JihiOuttaxGen = kaikeiInfModel.JihiOuttaxGen;
            AdjustFutan = kaikeiInfModel.AdjustFutan;
            AdjustRound = kaikeiInfModel.AdjustRound;
            TotalPtFutan = kaikeiInfModel.TotalPtFutan;
            AdjustFutanVal = kaikeiInfModel.AdjustFutanVal;
            AdjustFutanRange = kaikeiInfModel.AdjustFutanRange;
            AdjustRateVal = kaikeiInfModel.AdjustRateVal;
            AdjustRateRange = kaikeiInfModel.AdjustRateRange;
            CreateDate = kaikeiInfModel.CreateDate;
            CreateId = kaikeiInfModel.CreateId;
            CreateMachine = kaikeiInfModel.CreateMachine;
            Kohi1Priority = kaikeiInfModel.Kohi1Priority;
            Kohi2Priority = kaikeiInfModel.Kohi2Priority;
            Kohi3Priority = kaikeiInfModel.Kohi3Priority;
            Kohi4Priority = kaikeiInfModel.Kohi4Priority;
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

        public DateTime CreateDate { get; private set; }

        public int CreateId { get; private set; }

        public string CreateMachine { get; private set; }

        public string Kohi1Priority { get; private set; }

        public string Kohi2Priority { get; private set; }

        public string Kohi3Priority { get; private set; }

        public string Kohi4Priority { get; private set; }
    }
}
