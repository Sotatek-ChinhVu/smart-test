namespace Domain.Models.Receipt
{
    public class ReceInfForCheckErrSwapHokenModel
    {
        public ReceInfForCheckErrSwapHokenModel(long ptId, long ptNum, int sinYm, int hokenId)
        {
            PtId = ptId;
            PtNum = ptNum;
            SinYm = sinYm;
            HokenId = hokenId;
        }

        public long PtId { get; private set; }

        public long PtNum { get; private set; }

        public int SinYm { get; private set; }

        public int HokenId { get; private set; }
    }
}
