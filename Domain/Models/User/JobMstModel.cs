namespace Domain.Models.User
{
    public class JobMstModel
    {
        public JobMstModel(int hpId, int jobCd, string jobName, int sortNo)
        {
            HpId = hpId;
            JobCd = jobCd;
            JobName = jobName;
            SortNo = sortNo;
        }


        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        public int HpId { get; set; }

        /// <summary>
        /// 職種コード
        /// </summary>
        public int JobCd { get; set; }

        /// <summary>
        /// 職種名
        /// </summary>
        public string JobName { get; set; }

        /// <summary>
        /// 並び順
        /// </summary>
        public int SortNo { get; set; }
    }
}
