using Helper.Extension;

namespace Domain.Models.Accounting
{
    public class SinMeiModel
    {
        public SinMeiModel(int sinId, string sinIdBinding, string itemName, double suryo, string unitName, string tenKai, double totalTen, double totalKingaku, double kingaku, int futanS, int futanK1, int futanK2, int futanK3, int futanK4, string cdKbn, int jihiSbt, int enTenKbn, int santeiKbn, int inOutKbn, bool isRowColorGray)
        {
            SinId = sinId;
            SinIdBinding = sinIdBinding;
            ItemName = itemName;
            Suryo = suryo;
            UnitName = unitName;
            TenKai = tenKai;
            TotalTen = totalTen;
            TotalKingaku = totalKingaku;
            Kingaku = kingaku;
            FutanS = futanS;
            FutanK1 = futanK1;
            FutanK2 = futanK2;
            FutanK3 = futanK3;
            FutanK4 = futanK4;
            CdKbn = cdKbn;
            JihiSbt = jihiSbt;
            EnTenKbn = enTenKbn;
            SanteiKbn = santeiKbn;
            InOutKbn = inOutKbn;
            IsRowColorGray = isRowColorGray;
        }

        public SinMeiModel(int sinId, string sinIdBinding, string itemName, double suryo, string unitName, string tenKai, double totalTen, double totalKingaku, double kingaku, int futanS, int futanK1, int futanK2, int futanK3, int futanK4, string cdKbn, int jihiSbt, int enTenKbn, int santeiKbn, int inOutKbn, bool isRowColorGray, int day1, int day2, int day3, int day4, int day5, int day6, int day7, int day8, int day9, int day10, int day11, int day12, int day13, int day14, int day15, int day16, int day17, int day18, int day19, int day20, int day21, int day22, int day23, int day24, int day25, int day26, int day27, int day28, int day29, int day30, int day31) : this(sinId, sinIdBinding, itemName, suryo, unitName, tenKai, totalTen, totalKingaku, kingaku, futanS, futanK1, futanK2, futanK3, futanK4, cdKbn, jihiSbt, enTenKbn, santeiKbn, inOutKbn, isRowColorGray)
        {
            Day1 = day1;
            Day2 = day2;
            Day3 = day3;
            Day4 = day4;
            Day5 = day5;
            Day6 = day6;
            Day7 = day7;
            Day8 = day8;
            Day9 = day9;
            Day10 = day10;
            Day11 = day11;
            Day12 = day12;
            Day13 = day13;
            Day14 = day14;
            Day15 = day15;
            Day16 = day16;
            Day17 = day17;
            Day18 = day18;
            Day19 = day19;
            Day20 = day20;
            Day21 = day21;
            Day22 = day22;
            Day23 = day23;
            Day24 = day24;
            Day25 = day25;
            Day26 = day26;
            Day27 = day27;
            Day28 = day28;
            Day29 = day29;
            Day30 = day30;
            Day31 = day31;
        }

        public int SinId { get; private set; }
        public string SinIdBinding { get; set; }
        public string ItemName { get; private set; }
        public double Suryo { get; private set; }
        public string UnitName { get; private set; }
        public string TenKai { get; private set; }
        public double TotalTen { get; private set; }
        public double TotalKingaku { get; private set; }
        public double Kingaku { get; private set; }
        public int FutanS { get; private set; }
        public int FutanK1 { get; private set; }
        public int FutanK2 { get; private set; }
        public int FutanK3 { get; private set; }
        public int FutanK4 { get; private set; }
        public string CdKbn { get; private set; }
        public int JihiSbt { get; private set; }
        public int EnTenKbn { get; private set; }
        public int SanteiKbn { get; private set; }
        public int InOutKbn { get; private set; }
        public string Quantity { get => Suryo > 0 && !string.IsNullOrEmpty(UnitName) ? Suryo.AsString() + UnitName : ""; }
        public double SinHoTotalTen { get => EnTenKbn == 1 ? Kingaku / 10 : TotalTen; }
        public double Total { get => TotalKingaku != 0 ? TotalKingaku : TotalTen; }
        public string TotalBinding { get => Total != 0 ? Total.AsString() : ""; }
        public string FutanSBinding { get => FutanS == 1 ? "＊" : ""; }
        public string FutanK1Binding { get => FutanK1 >= 1 ? "＊" : ""; }
        public string FutanK2Binding { get => FutanK2 >= 1 ? "＊" : ""; }
        public string FutanK3Binding { get => FutanK3 >= 1 ? "＊" : ""; }
        public string FutanK4Binding { get => FutanK4 >= 1 ? "＊" : ""; }
        public string Asterisk { get => SinId > 0 ? "＊" : ""; }
        public bool IsRowColorGray { get; private set; } = false;
        public bool IsForegroundRed { get => EnTenKbn == 1; }
        public int Day1 { get; private set; }
        public int Day2 { get; private set; }
        public int Day3 { get; private set; }
        public int Day4 { get; private set; }
        public int Day5 { get; private set; }
        public int Day6 { get; private set; }
        public int Day7 { get; private set; }
        public int Day8 { get; private set; }
        public int Day9 { get; private set; }
        public int Day10 { get; private set; }
        public int Day11 { get; private set; }
        public int Day12 { get; private set; }
        public int Day13 { get; private set; }
        public int Day14 { get; private set; }
        public int Day15 { get; private set; }
        public int Day16 { get; private set; }
        public int Day17 { get; private set; }
        public int Day18 { get; private set; }
        public int Day19 { get; private set; }
        public int Day20 { get; private set; }
        public int Day21 { get; private set; }
        public int Day22 { get; private set; }
        public int Day23 { get; private set; }
        public int Day24 { get; private set; }
        public int Day25 { get; private set; }
        public int Day26 { get; private set; }
        public int Day27 { get; private set; }
        public int Day28 { get; private set; }
        public int Day29 { get; private set; }
        public int Day30 { get; private set; }
        public int Day31 { get; private set; }
    }
}
