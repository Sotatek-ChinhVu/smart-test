namespace Helper.Messaging.Data
{
    public class RecalculationStatus
    {
        public bool Done { get; set; }

        public int Type { get; set; }

        public int Length { get; set; }

        public int SuccessCount { get; set; }

        public string Message { get; set; }
    }
}
