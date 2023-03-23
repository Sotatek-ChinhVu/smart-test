namespace EmrCloudApi.Requests.WeightedSetConfirmation
{
    public class SaveWeightedSetConfirmationRequest
    {
        public SaveWeightedSetConfirmationRequest(long ptId, long raiinNo, double weithgt, int sinDate)
        {
            PtId = ptId;
            RaiinNo = raiinNo;
            Weithgt = weithgt;
            SinDate = sinDate;
        }

        public long PtId { get; private set; }

        public long RaiinNo { get; private set; }

        public double Weithgt { get; private set; }

        public int SinDate { get; private set; }
    }
}
