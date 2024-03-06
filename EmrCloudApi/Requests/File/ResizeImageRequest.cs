namespace EmrCloudApi.Requests.File
{
    public class ResizeImageRequest
    {
        public string ImagePath { get; set; } = string.Empty;

        public int Height { get; set; }
    }
}
