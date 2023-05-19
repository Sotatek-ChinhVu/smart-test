namespace Domain.Models.MstItem
{
    public class SearchSupplementModel
    {
        public SearchSupplementModel(string seibunCd, string seibun, string indexWord, string tokuhoFlg, string indexCd, string seibunGroupByIndexCd)
        {
            SeibunCd = seibunCd;
            Seibun = seibun;
            IndexWord = indexWord;
            TokuhoFlg = tokuhoFlg;
            IndexCd = indexCd;
            SeibunGroupByIndexCd = seibunGroupByIndexCd;
        }

        public string SeibunCd { get; private set; }

        public string Seibun { get; private set; }

        public string IndexWord { get; private set; }

        public string TokuhoFlg { get; private set; }

        public string TokuhoFlgConvert
        {
            get
            {
                if (TokuhoFlg == "1")
                {
                    return "●";
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public string IndexCd { get; private set; }

        public string SeibunGroupByIndexCd { get; set; }
    }
}
