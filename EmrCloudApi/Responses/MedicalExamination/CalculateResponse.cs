namespace EmrCloudApi.Responses.MedicalExamination
{
    public class CalculateResponse
    {
        public CalculateResponse(long ptId, int sinDate)
        {
            PtId = ptId;
            SinDate = sinDate;
        }

        public long PtId { get; private set; }

        public int SinDate { get; private set; }
    }
}
