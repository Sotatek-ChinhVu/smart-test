namespace EmrCloudApi.Responses.Todo
{
    public class UpsertTodoGrpMstResponse
    {
        public UpsertTodoGrpMstResponse(bool success)
        {
            Success = success;
        }
        public bool Success { get; private set; }
    }
}
