﻿namespace Domain.Models.KensaIrai
{
    public class ListKensaInfDetailModel
    {
        public ListKensaInfDetailModel()
        {
            KensaInfDetailCol = new List<KensaInfDetailColModel>();
            KensaInfDetailData = new List<KensaInfDetailDataModel>();
        }
        public ListKensaInfDetailModel(List<KensaInfDetailColModel> kensaInfDetailCol, List<KensaInfDetailDataModel> kensaInfDetailData)
        {
            KensaInfDetailCol = kensaInfDetailCol;
            KensaInfDetailData = kensaInfDetailData;
        }

        public List<KensaInfDetailColModel> KensaInfDetailCol { get; private set; }

        public List<KensaInfDetailDataModel> KensaInfDetailData { get; private set; }

        public class KensaInfDetailColModel
        {
            public KensaInfDetailColModel(long iraiCd, long iraiDate, string nyubi, string yoketu, int bilirubin, int sikyuKbn, int tosekiKbn)
            {
                IraiCd = iraiCd;
                IraiDate = iraiDate;
                Nyubi = nyubi;
                Yoketu = yoketu;
                Bilirubin = bilirubin;
                SikyuKbn = sikyuKbn;
                TosekiKbn = tosekiKbn;
            }

            public long IraiCd { get; private set; }

            public long IraiDate { get; private set; }

            public string Nyubi { get; private set; }

            public string Yoketu { get; private set; }

            public int Bilirubin { get; private set; }

            public int SikyuKbn { get; private set; }

            public int TosekiKbn { get; private set; }
        }

        public class KensaInfDetailDataModel
        {
            public KensaInfDetailDataModel(string kensaItemCd, string kensaName, string unit, string std, List<ListKensaInfDetailItemModel> dynamicArray)
            {
                KensaItemCd = kensaItemCd;
                KensaName = kensaName;
                Unit = unit;
                Std = std;
                DynamicArray = dynamicArray;
            }

            public string KensaItemCd { get; private set; }

            public string KensaName { get; private set; }

            public string Unit { get; private set; }

            public string Std { get; private set; }

            public List<ListKensaInfDetailItemModel> DynamicArray { get; private set; }
        }
    }
}