namespace EmrCloudApi.Responses.User
{
    public class CheckedLockMedicalExaminationResponse
    {
        public CheckedLockMedicalExaminationResponse(bool isLocked)
        {
            IsLocked = isLocked;
        }

        public bool IsLocked { get; private set; }
    }
}
