namespace EmrCloudApi.Responses.MedicalExamination
{
    public class GetHistoryIndexResponse
    {
        public GetHistoryIndexResponse(long index)
        {
            Index = index;
        }

        public long Index { get; private set; }
    }
}
