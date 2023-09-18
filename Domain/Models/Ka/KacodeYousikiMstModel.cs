namespace Domain.Models.Ka
{
    public class KacodeYousikiMstModel
    {
        public KacodeYousikiMstModel(string yousikiKaCd, int sortNo, string kaName)
        {
            YousikiKaCd = yousikiKaCd;
            SortNo = sortNo;
            KaName = kaName;
        }

        public string YousikiKaCd{ get; private set; }

        public int SortNo{ get; private set; }

        public string KaName{ get; private set; }

        public string DisplayYousikiKaCd
        {
            get => YousikiKaCd + " " + KaName;
        }
    }
}
