namespace EmrCloudApi.Requests.Todo
{
    public class GetTodoInfFinderRequest
    {
        public int TodoNo { get; set; }

        public int TodoEdaNo { get; set; }

        public bool IncDone { get; set; }

        public bool SortByPtNum { get; set; }
    }
}
