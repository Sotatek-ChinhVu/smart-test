namespace Domain.Models.CalculateModel
{
    public class KaikeiInfDataModelDto
    {
        public List<KaikeiInfDataModel> KaikeiInfList { get; set; } = new();
    }
    public class KaikeiInfDataModel
    {
        public int hpId { get; set; }
        public int ptId { get; set; }
        public int sinDate { get; set; }
        public int raiinNo { get; set; }
        public int hokenId { get; set; }
        public int kohi1Id { get; set; }
        public int kohi2Id { get; set; }
        public int kohi3Id { get; set; }
        public int kohi4Id { get; set; }
        public int hokenKbn { get; set; }
        public int hokenSbtCd { get; set; }
        public string receSbt { get; set; } = string.Empty;
        public string houbetu { get; set; } = string.Empty;
        public string kohi1Houbetu { get; set; } = string.Empty;
        public string kohi2Houbetu { get; set; } = string.Empty;
        public string kohi3Houbetu { get; set; } = string.Empty;
        public string kohi4Houbetu { get; set; } = string.Empty;
        public int honkeKbn { get; set; }
        public int hokenRate { get; set; }
        public int ptRate { get; set; }
        public int dispRate { get; set; }
        public int tensu { get; set; }
        public int totalIryohi { get; set; }
        public int ptFutan { get; set; }
        public int jihiFutan { get; set; }
        public int jihiTax { get; set; }
        public int jihiOuttax { get; set; }
        public int jihiFutanTaxfree { get; set; }
        public int jihiFutanTaxNr { get; set; }
        public int jihiFutanTaxGen { get; set; }
        public int jihiFutanOuttaxNr { get; set; }
        public int jihiFutanOuttaxGen { get; set; }
        public int jihiTaxNr { get; set; }
        public int jihiTaxGen { get; set; }
        public int jihiOuttaxNr { get; set; }
        public int jihiOuttaxGen { get; set; }
        public int adjustFutan { get; set; }
        public int adjustRound { get; set; }
        public int totalPtFutan { get; set; }
        public int adjustFutanVal { get; set; }
        public int adjustFutanRange { get; set; }
        public int adjustRateVal { get; set; }
        public int adjustRateRange { get; set; }
        public DateTime createDate { get; set; }
        public int createId { get; set; }
        public string createMachine { get; set; } = string.Empty;
        public string kohi1Priority { get; set; } = string.Empty;
        public string kohi2Priority { get; set; } = string.Empty;
        public string kohi3Priority { get; set; } = string.Empty;
        public string kohi4Priority { get; set; } = string.Empty;
    }
}
