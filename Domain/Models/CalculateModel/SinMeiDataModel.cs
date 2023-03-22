namespace Domain.Models.CalculateModel
{
    public class SinMeiDataModelDto
    {
        public List<SinMeiDataModel> sinMeiList { get; set; } = new();
    }
    public class SinMeiDataModel
    {
        public long PtId { get; set; }
        public string RecId { get; set; } = string.Empty;
        public int SinId { get; set; }
        public int SinIdOrg { get; set; }
        public string FutanKbn { get; set; } = string.Empty;
        public int FutanSortNo { get; set; }
        public string ItemCd { get; set; } = string.Empty;
        public string OdrItemCd { get; set; } = string.Empty;
        public string ItemName { get; set; } = string.Empty;
        public string CommentData { get; set; } = string.Empty;
        public double Suryo { get; set; }
        public string SuryoDsp { get; set; } = string.Empty;
        public double TotalTen { get; set; }
        public double TotalKingaku { get; set; }
        public double Ten { get; set; }
        public double Kingaku { get; set; }
        public int Count { get; set; }
        public string TenKai { get; set; } = string.Empty;
        public bool DspZeroTenKai { get; set; }
        public int UnitCd { get; set; }
        public string UnitName { get; set; } = string.Empty;
        public double Price { get; set; }
        public string TokuzaiName { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string Comment1 { get; set; } = string.Empty;
        public string CommentCd1 { get; set; } = string.Empty;
        public string CommentData1 { get; set; } = string.Empty;
        public string Comment2 { get; set; } = string.Empty;
        public string CommentCd2 { get; set; } = string.Empty;
        public string CommentData2 { get; set; } = string.Empty;
        public string Comment3 { get; set; } = string.Empty;
        public string CommentCd3 { get; set; } = string.Empty;
        public string CommentData3 { get; set; } = string.Empty;
        public int Day1 { get; set; }
        public int Day2 { get; set; }
        public int Day3 { get; set; }
        public int Day4 { get; set; }
        public int Day5 { get; set; }
        public int Day6 { get; set; }
        public int Day7 { get; set; }
        public int Day8 { get; set; }
        public int Day9 { get; set; }
        public int Day10 { get; set; }
        public int Day11 { get; set; }
        public int Day12 { get; set; }
        public int Day13 { get; set; }
        public int Day14 { get; set; }
        public int Day15 { get; set; }
        public int Day16 { get; set; }
        public int Day17 { get; set; }
        public int Day18 { get; set; }
        public int Day19 { get; set; }
        public int Day20 { get; set; }
        public int Day21 { get; set; }
        public int Day22 { get; set; }
        public int Day23 { get; set; }
        public int Day24 { get; set; }
        public int Day25 { get; set; }
        public int Day26 { get; set; }
        public int Day27 { get; set; }
        public int Day28 { get; set; }
        public int Day29 { get; set; }
        public int Day30 { get; set; }
        public int Day31 { get; set; }
        public int Day1Add { get; set; }
        public int Day2Add { get; set; }
        public int Day3Add { get; set; }
        public int Day4Add { get; set; }
        public int Day5Add { get; set; }
        public int Day6Add { get; set; }
        public int Day7Add { get; set; }
        public int Day8Add { get; set; }
        public int Day9Add { get; set; }
        public int Day10Add { get; set; }
        public int Day11Add { get; set; }
        public int Day12Add { get; set; }
        public int Day13Add { get; set; }
        public int Day14Add { get; set; }
        public int Day15Add { get; set; }
        public int Day16Add { get; set; }
        public int Day17Add { get; set; }
        public int Day18Add { get; set; }
        public int Day19Add { get; set; }
        public int Day20Add { get; set; }
        public int Day21Add { get; set; }
        public int Day22Add { get; set; }
        public int Day23Add { get; set; }
        public int Day24Add { get; set; }
        public int Day25Add { get; set; }
        public int Day26Add { get; set; }
        public int Day27Add { get; set; }
        public int Day28Add { get; set; }
        public int Day29Add { get; set; }
        public int Day30Add { get; set; }
        public int Day31Add { get; set; }
        public int HokenPid { get; set; }
        public int RpNo { get; set; }
        public int SeqNo { get; set; }
        public int RowNo { get; set; }
        public string CdKbn { get; set; } = string.Empty;
        public int JihiSbt { get; set; }
        public int FutanS { get; set; }
        public int FutanK1 { get; set; }
        public int FutanK2 { get; set; }
        public int FutanK3 { get; set; }
        public int FutanK4 { get; set; }
        public int LastRowKbn { get; set; }
        public int SanteiKbn { get; set; }
        public int InOutKbn { get; set; }
        public int KizamiId { get; set; }
        public int TenId { get; set; }
        public int KazeiKbn { get; set; }
        public int ZeiKigoEdaNo { get; set; }
        public int ZeiInOut { get; set; }
        public int TaxRate { get; set; }
        public string RecedenRecord { get; set; } = string.Empty;
        public string RousaiRecedenRecord { get; set; } = string.Empty;
        public string AfterCareRecedenRecord { get; set; } = string.Empty;
        public int ReceSortKey { get; set; }
        public int FutanSortKey { get; set; }
        public string SyukeiSaki { get; set; } = string.Empty;
        public int EnTenKbn { get; set; }
        public int DrugKbn { get; set; }
        public int SinRpNo { get; set; }
        public int SinSeqNo { get; set; }
        public bool IsComment { get; set; }
    }
}
