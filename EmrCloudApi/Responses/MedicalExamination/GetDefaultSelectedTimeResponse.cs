namespace EmrCloudApi.Responses.MedicalExamination
{
    public class GetDefaultSelectedTimeResponse
    {
        public GetDefaultSelectedTimeResponse(int value, string message, int timeKbnForChild, int currentTimeKbn, int beforeTimeKbn)
        {
            Value = value;
            Message = message;
            TimeKbnForChild = timeKbnForChild;
            CurrentTimeKbn = currentTimeKbn;
            BeforeTimeKbn = beforeTimeKbn;
        }

        public int Value { get; private set; }

        public string Message { get; private set; }

        public int TimeKbnForChild { get; private set; }

        public int CurrentTimeKbn { get; private set; }

        public int BeforeTimeKbn { get; private set; }
    }
}
