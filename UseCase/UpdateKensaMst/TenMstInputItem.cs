namespace UseCase.UpdateKensaMst
{
    public class TenMstInputItem
    {
        public TenMstInputItem(int sinKouiKbn, string masterSbt, string itemCd, string kensaItemCd, int kensaItemSeqNo, double ten, string name, string receName, string kanaName1,
                               string kanaName2, string kanaName3, string kanaName4, string kanaName5, string kanaName6, string kanaName7, int startDate, int endDate, double defaultValue,
                               string odrUnitName, string santeiItemCd, int santeigaiKbn, int isNoSearch, int isDeleted)
        {

            SinKouiKbn = sinKouiKbn;
            MasterSbt = masterSbt;
            ItemCd = itemCd;
            KensaItemCd = kensaItemCd;
            KensaItemSeqNo = kensaItemSeqNo;
            Ten = ten;
            Name = name;
            ReceName = receName;
            KanaName1 = kanaName1;
            KanaName2 = kanaName2;
            KanaName3 = kanaName3;
            KanaName4 = kanaName4;
            KanaName5 = kanaName5;
            KanaName6 = kanaName6;
            KanaName7 = kanaName7;
            StartDate = startDate;
            EndDate = endDate;
            DefaultValue = defaultValue;
            OdrUnitName = odrUnitName;
            SanteiItemCd = santeiItemCd;
            SanteigaiKbn = santeigaiKbn;
            IsNoSearch = isNoSearch;
            IsDeleted = isDeleted;
        }

        public int SinKouiKbn { get; private set; }

        public string MasterSbt { get; private set; }

        public string ItemCd { get; private set; }

        public string KensaItemCd { get; private set; }

        public int KensaItemSeqNo { get; private set; }

        public double Ten { get; private set; }

        public string Name { get; private set; }

        public string ReceName { get; private set; }

        public string KanaName1 { get; private set; }

        public string KanaName2 { get; private set; }

        public string KanaName3 { get; private set; }

        public string KanaName4 { get; private set; }

        public string KanaName5 { get; private set; }

        public string KanaName6 { get; private set; }

        public string KanaName7 { get; private set; }

        public int StartDate { get; private set; }

        public int EndDate { get; private set; }

        public double DefaultValue { get; private set; }

        public string OdrUnitName { get; private set; }

        public string SanteiItemCd { get; private set; }

        public int SanteigaiKbn { get; private set; }

        public int IsNoSearch { get; private set; }

        public int IsDeleted { get; private set; }
    }
}