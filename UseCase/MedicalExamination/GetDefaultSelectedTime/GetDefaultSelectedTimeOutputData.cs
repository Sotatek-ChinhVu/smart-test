using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.GetDefaultSelectedTime
{
    public class GetDefaultSelectedTimeOutputData : IOutputData
    {
        public GetDefaultSelectedTimeOutputData(GetDefaultSelectedTimeStatus status, int value, string message, int timeKbnForChild, int currentTimeKbn, int beforeTimeKbn)
        {
            Status = status;
            Value = value;
            Message = message;
            TimeKbnForChild = timeKbnForChild;
            CurrentTimeKbn = currentTimeKbn;
            BeforeTimeKbn = beforeTimeKbn;
        }

        public GetDefaultSelectedTimeStatus Status { get; private set; }

        public int Value { get; private set; }

        public string Message { get; private set; }

        public int TimeKbnForChild { get; private set; }

        public int CurrentTimeKbn { get; private set; }

        public int BeforeTimeKbn { get; private set; }

    }
}
