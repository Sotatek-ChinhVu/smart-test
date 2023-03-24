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

        public SinMeiModel(int sinId, string sinIdBinding, string itemName, double suryo, string unitName, string tenKai, double totalTen, double totalKingaku, double kingaku, int futanS, int futanK1, int futanK2, int futanK3, int futanK4, string cdKbn, int jihiSbt, int enTenKbn, int santeiKbn, int inOutKbn, bool isRowColorGray, List<int> days)
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
            Days = days;
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
        public List<int> Days { get; private set; }
    }
}
