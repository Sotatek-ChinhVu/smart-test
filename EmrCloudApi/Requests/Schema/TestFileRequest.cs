namespace EmrCloudApi.Requests.Schema
{
    public class TestFileRequest
    {
        public string Id { get; set; }

        public IFormFile File { get; set; }

        public string Folder { get; set; }
    }
}
