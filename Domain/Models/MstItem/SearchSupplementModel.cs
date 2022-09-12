namespace Domain.Models.MstItem
{
    public class SearchSupplementModel 
    {
        public string SeibunCd { get; set; } = string.Empty;
        
        public string Seibun { get; set; } = string.Empty;

        public string IndexWord { get; set; } = string.Empty;
        public string TokuhoFlg { get; set; } = string.Empty;
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
        public string IndexCd { get; set; } = string.Empty;
        public string SeibunGroupByIndexCd { get; set; } = string.Empty;
    }
}
