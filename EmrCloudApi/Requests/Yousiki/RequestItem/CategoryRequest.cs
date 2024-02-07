namespace EmrCloudApi.Requests.Yousiki.RequestItem
{
    public class CategoryRequest
    {
        public int DataType { get; set; }

        /// <summary>
        /// 0 : Update
        /// 1 : delete
        /// 2 : add new
        /// </summary>
        public int IsDeleted { get; set; }
    }
}
