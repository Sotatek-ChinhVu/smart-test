namespace EmrCloudApi.Requests.Todo
{
    public class GetTodoInfRequest
    {
        public int TodoNo { get; set; }

        public int TodoEdaNo { get; set; }

        public bool IncDone { get; set; }
    }
}
