namespace EmrCloudApi.Responses.Todo
{
    public class UpsertTodoInfResponse
    {
        public UpsertTodoInfResponse(bool success)
        {
            Success = success;
        }

        public bool Success { get; private set; }
    }
}