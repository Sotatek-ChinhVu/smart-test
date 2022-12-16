namespace Domain.Models.Insurance
{
    public class KohiPriorityModel
    {
        public KohiPriorityModel(string priorityNo, int prefNo, string houbetu)
        {
            PriorityNo = priorityNo;
            PrefNo = prefNo;
            Houbetu = houbetu;
        }

        /// <summary>
        /// 優先順位
        /// </summary>
        public string PriorityNo { get; private set; }

        /// <summary>
        /// 都道府県番号
        /// </summary>
        public int PrefNo { get; private set; }

        /// <summary>
        /// 法別番号
        /// </summary>
        public string Houbetu { get; private set; }
    }
}
