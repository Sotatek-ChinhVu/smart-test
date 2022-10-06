namespace UseCase.OrdInfs.ValidationInputItem
{
    public class ValidationInputItemDetailItem
    {
        public int RowNo { get; private set; }
        public int SinKouiKbn { get; private set; }
        public string ItemCd { get; private set; }
        public string ItemName { get; private set; }
        public double Suryo { get; private set; }
        public string UnitName { get; private set; }
        public int KohatuKbn { get; private set; }
        public int SyohoKbn { get; private set; }
        public int DrugKbn { get; private set; }
        public int YohoKbn { get; private set; }
        public string Bunkatu { get; private set; }
        public string CmtName { get; private set; }
        public string CmtOpt { get; private set; }

        public ValidationInputItemDetailItem(int sinKouiKbn, string itemCd, string itemName, double suryo, string unitName, int kohatuKbn, int syohoKbn, int drugKbn, int yohoKbn, string bunkatu, string cmtName, string cmtOpt)
        {
            SinKouiKbn = sinKouiKbn;
            ItemCd = itemCd;
            ItemName = itemName;
            Suryo = suryo;
            UnitName = unitName;
            KohatuKbn = kohatuKbn;
            SyohoKbn = syohoKbn;
            DrugKbn = drugKbn;
            YohoKbn = yohoKbn;
            Bunkatu = bunkatu;
            CmtName = cmtName;
            CmtOpt = cmtOpt;
        }
    }
}
