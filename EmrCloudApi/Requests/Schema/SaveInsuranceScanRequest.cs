namespace EmrCloudApi.Requests.Schema
{
    public class SaveInsuranceScanRequest
    {
        public long PtId { get; set; }

        public int HokenGrp { get; set; }

        public int HokenId { get; set; }

        public long SeqNo { get; set; }

        /// <summary>
        /// Url Image
        /// </summary>
        public string FileName { get; set; } = string.Empty;

        public IFormFile? File { get; set; }

        public int IsDeleted { get; set; }
    }
}
