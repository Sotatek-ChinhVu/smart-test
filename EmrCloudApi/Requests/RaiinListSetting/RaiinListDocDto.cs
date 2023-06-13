namespace EmrCloudApi.Requests.RaiinListSetting
{
    public class RaiinListDocDto
    {
        public RaiinListDocDto(int hpId, int grpId, int kbnCd, long seqNo, int categoryCd, string categoryName, int isDeleted)
        {
            HpId = hpId;
            GrpId = grpId;
            KbnCd = kbnCd;
            SeqNo = seqNo;
            CategoryCd = categoryCd;
            CategoryName = categoryName;
            IsDeleted = isDeleted;
        }

        public int HpId { get; private set; }

        public int GrpId { get; private set; }

        public int KbnCd { get; private set; }

        public long SeqNo { get; private set; }

        public int CategoryCd { get; private set; }

        /// <summary>
        /// Docategory model
        /// </summary>
        public string CategoryName { get; private set; }

        public int IsDeleted { get; private set; }
    }
}
