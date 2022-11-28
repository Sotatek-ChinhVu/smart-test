namespace EmrCloudApi.Responses
{
    public class Response<T>
    {
        public T Data { get; set; } = default!;

        public string Message { get; set; } = string.Empty;

        public int Status { get; set; }
    }

    public class Response
    {
        public string Message { get; set; } = string.Empty;

        public int Status { get; set; }
    }
}
