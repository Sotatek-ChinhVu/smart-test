namespace CommonCheckers.OrderRealtimeChecker.Models
{
    public class RealTimeCheckerCondition
    {
        public bool IsCheckingDuplication { get; private set; } = true;
        public bool IsCheckingKinki { get; private set; } = true;
        public bool IsCheckingAllergy { get; private set; } = true;
        public bool IsCheckingDosage { get; private set; } = true;
        public bool IsCheckingDays { get; private set; } = true;
        public bool IsCheckingAge { get; private set; } = true;
        public bool IsCheckingDisease { get; private set; } = true;
        public bool IsCheckingInvalidData { get; private set; } = true;
        public bool IsCheckingAutoCheck { get; private set; } = true;

        public RealTimeCheckerCondition()
        {
        }

        public RealTimeCheckerCondition(bool isCheckingDuplication, bool isCheckingKinki,
            bool isCheckingAllergy, bool isCheckingDosage,
            bool isCheckingDays, bool isCheckingAge,
            bool isCheckingDisease, bool isCheckingInvalidData,
            bool isCheckingAutoCheck)
        {
            IsCheckingDuplication = isCheckingDuplication;
            IsCheckingKinki = isCheckingKinki;
            IsCheckingAllergy = isCheckingAllergy;
            IsCheckingDosage = isCheckingDosage;
            IsCheckingDays = isCheckingDays;
            IsCheckingAge = isCheckingAge;
            IsCheckingDisease = isCheckingDisease;
            IsCheckingInvalidData = isCheckingInvalidData;
            IsCheckingAutoCheck = isCheckingAutoCheck;
        }
    }
}
