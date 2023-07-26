namespace EmrCloudApi.Responses.Reception
{
    public class RevertDeleteNoReceptResponse
    {
        public RevertDeleteNoReceptResponse(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }

        public bool IsSuccess { get; private set; }
    }
}
