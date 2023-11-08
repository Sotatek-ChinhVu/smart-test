namespace Domain.Models.KensaIrai
{
    public class ListKensaInfDetailModel
    {
        public ListKensaInfDetailModel()
        {
            KensaInfDetailCol = new List<KensaInfDetailColModel>();
            KensaInfDetailData = new List<KensaInfDetailDataModel>();
        }
        public ListKensaInfDetailModel(List<KensaInfDetailColModel> kensaInfDetailCol, List<KensaInfDetailDataModel> kensaInfDetailData, int totalCol)
        {
            KensaInfDetailCol = kensaInfDetailCol;
            KensaInfDetailData = kensaInfDetailData;
            TotalCol = totalCol;
        }

        public List<KensaInfDetailColModel> KensaInfDetailCol { get; private set; }

        public List<KensaInfDetailDataModel> KensaInfDetailData { get; private set; }
        public int TotalCol { get; private set; }

        public class KensaInfDetailColModel
        {
            public KensaInfDetailColModel(long iraiCd, long iraiDate, string nyubi, string yoketu, string bilirubin, int sikyuKbn, int tosekiKbn, int index)
            {
                IraiCd = iraiCd;
                IraiDate = iraiDate;
                Nyubi = nyubi;
                Yoketu = yoketu;
                Bilirubin = bilirubin;
                SikyuKbn = sikyuKbn;
                TosekiKbn = tosekiKbn;
                Index = index;
            }

            public long IraiCd { get; private set; }

            public long IraiDate { get; private set; }

            public string Nyubi { get; private set; }

            public string Yoketu { get; private set; }

            public string Bilirubin { get; private set; }

            public int SikyuKbn { get; private set; }

            public int TosekiKbn { get; private set; }

            public int Index { get; private set; }

            public void SetIndex(int newIndex)
            {
                Index = newIndex;
            }
        }

        public class KensaInfDetailDataModel
        {
            public KensaInfDetailDataModel(string kensaItemCd, string kensaName, string unit, string maleStd, string femaleStd, List<ListKensaInfDetailItemModel> dynamicArray)
            {
                KensaItemCd = kensaItemCd;
                KensaName = kensaName;
                Unit = unit;
                MaleStd = maleStd;
                FemaleStd = femaleStd;
                DynamicArray = dynamicArray;
            }

            public KensaInfDetailDataModel(string kensaItemCd, string kensaName, string unit, string maleStd, string femaleStd, string kensaKana, long sortNo, long seqNo, long seqParentNo, List<ListKensaInfDetailItemModel> dynamicArray)
            {
                KensaItemCd = kensaItemCd;
                KensaName = kensaName;
                Unit = unit;
                MaleStd = maleStd;
                FemaleStd = femaleStd;
                KensaKana = kensaKana;
                SortNo = sortNo;
                SeqNo = seqNo;
                SeqParentNo = seqParentNo;
                DynamicArray = dynamicArray;
            }


            public string KensaItemCd { get; private set; }

            public string KensaName { get; private set; }

            public string Unit { get; private set; }

            public string MaleStd { get; private set; }

            public string FemaleStd { get; private set; }

            public string KensaKana { get; private set; }

            public long SortNo { get; private set; }

            public long SeqNo { get; private set; }

            public long SeqParentNo { get; private set; }

            public List<ListKensaInfDetailItemModel> DynamicArray { get; private set; }
        }
    }
}
