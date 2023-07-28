namespace EmrCloudApi.Requests.Receipt
{
    public class CheckExisReceInfEditRequest
    {
        public CheckExisReceInfEditRequest(int seikyuYm, long ptId, int sinYm, int hokenId)
        {
            SeikyuYm = seikyuYm;
            PtId = ptId;
            SinYm = sinYm;
            HokenId = hokenId;
        }

        public int SeikyuYm { get; private set; }

        public long PtId { get; private set; }

        public int SinYm { get; private set; }

        public int HokenId { get; private set; }

    }
}
