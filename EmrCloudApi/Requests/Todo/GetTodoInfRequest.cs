namespace EmrCloudApi.Requests.Todo
{
    public class GetTodoInfRequest
    {
        public int TodoNo { get; set; }

        public int TodoEdaNo { get; set; }

        public int PtId { get; set; }

        public int IsDone { get; set; } 
    }
}
