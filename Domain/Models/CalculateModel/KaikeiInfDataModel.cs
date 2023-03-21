namespace Domain.Models.CalculateModel
{
    public class KaikeiInfDataModelDto
    {
        public List<KaikeiInfDataModel> KaikeiInfList { get; set; } = new();
    }
    public class KaikeiInfDataModel
    {
        public int HpId { get; set; }
        public int PtId { get; set; }
        public int SinDate { get; set; }
        public int RaiinNo { get; set; }
        public int HokenId { get; set; }
        public int Kohi1Id { get; set; }
        public int Kohi2Id { get; set; }
        public int Kohi3Id { get; set; }
        public int Kohi4Id { get; set; }
        public int HokenKbn { get; set; }
        public int HokenSbtCd { get; set; }
        public string ReceSbt { get; set; } = string.Empty;
        public string Houbetu { get; set; } = string.Empty;
        public string Kohi1Houbetu { get; set; } = string.Empty;
        public string Kohi2Houbetu { get; set; } = string.Empty;
        public string Kohi3Houbetu { get; set; } = string.Empty;
        public string Kohi4Houbetu { get; set; } = string.Empty;
        public int HonkeKbn { get; set; }
        public int HokenRate { get; set; }
        public int PtRate { get; set; }
        public int DispRate { get; set; }
        public int Tensu { get; set; }
        public int TotalIryohi { get; set; }
        public int PtFutan { get; set; }
        public int JihiFutan { get; set; }
        public int JihiTax { get; set; }
        public int JihiOuttax { get; set; }
        public int JihiFutanTaxfree { get; set; }
        public int JihiFutanTaxNr { get; set; }
        public int JihiFutanTaxGen { get; set; }
        public int JihiFutanOuttaxNr { get; set; }
        public int JihiFutanOuttaxGen { get; set; }
        public int JihiTaxNr { get; set; }
        public int JihiTaxGen { get; set; }
        public int JihiOuttaxNr { get; set; }
        public int JihiOuttaxGen { get; set; }
        public int AdjustFutan { get; set; }
        public int AdjustRound { get; set; }
        public int TotalPtFutan { get; set; }
        public int AdjustFutanVal { get; set; }
        public int AdjustFutanRange { get; set; }
        public int AdjustRateVal { get; set; }
        public int AdjustRateRange { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateId { get; set; }
        public string CreateMachine { get; set; } = string.Empty;
        public string Kohi1Priority { get; set; } = string.Empty;
        public string Kohi2Priority { get; set; } = string.Empty;
        public string Kohi3Priority { get; set; } = string.Empty;
        public string Kohi4Priority { get; set; } = string.Empty;
    }
}
