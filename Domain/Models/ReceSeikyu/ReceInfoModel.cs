namespace Domain.Models.ReceSeikyu
{
    public class ReceInfo
    {
        public ReceInfo(long ptId, int hokenId, int sinYm, int seikyuYm)
        {
            PtId = ptId;
            HokenId = hokenId;
            SinYm = sinYm;
            SeikyuYm = seikyuYm;
        }

        public long PtId { get; private set; }

        public int HokenId { get; private set; }

        public int SinYm { get; private set; }

        public int SeikyuYm { get; private set; }
    }
}
