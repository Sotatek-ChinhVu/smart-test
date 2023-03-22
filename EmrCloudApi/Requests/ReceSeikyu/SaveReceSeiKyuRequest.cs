namespace EmrCloudApi.Requests.ReceSeikyu
{
    public class SaveReceSeiKyuRequest
    {
        public SaveReceSeiKyuRequest(List<ReceSeikyuDto> data, int sinYm)
        {
            Data = data;
            SinYm = sinYm;
        }

        /// <summary>
        /// only pass modified record (IsModified = true)
        /// </summary>
        public List<ReceSeikyuDto> Data { get; private set; }

        public int SinYm { get; private set; }
    }
}
