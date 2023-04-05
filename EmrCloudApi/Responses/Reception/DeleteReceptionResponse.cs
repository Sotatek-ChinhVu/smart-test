namespace EmrCloudApi.Responses.Reception
{
    public class DeleteReceptionResponse
    {
        public DeleteReceptionResponse(bool status)
        {
            Status = status;
        }

        public bool Status { get; private set; }
    }
}
