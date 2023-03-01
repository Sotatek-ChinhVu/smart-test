using Entity.Tenant;

namespace EmrCalculateApi.ReceFutan.Models
{
    public class KogakuLimitModel
    {
        public KogakuLimit KogakuLimit { get; }

        public KogakuLimitModel(KogakuLimit kogakuLimit)
        {
            KogakuLimit = kogakuLimit;
        }

        /// <summary>
        /// 年齢区分
        /// </summary>
        public int AgeKbn
        {
            get { return KogakuLimit.AgeKbn; }
        }

        /// <summary>
        /// 高額療養費区分
        /// </summary>
        public int KogakuKbn
        {
            get { return KogakuLimit.KogakuKbn; }
        }

        /// <summary>
        /// 開始日
        /// </summary>
        public int StartDate
        {
            get { return KogakuLimit.StartDate; }
        }

        /// <summary>
        /// 終了日
        /// </summary>
        public int EndDate
        {
            get { return KogakuLimit.EndDate; }
        }

        /// <summary>
        /// 基準額
        /// </summary>
        public int BaseLimit
        {
            get { return KogakuLimit.BaseLimit; }
        }

        /// <summary>
        /// 調整額
        /// </summary>
        public int AdjustLimit
        {
            get { return KogakuLimit.AdjustLimit; }
        }

        /// <summary>
        /// 多数該当
        /// </summary>
        public int TasuLimit
        {
            get { return KogakuLimit.TasuLimit; }
        }

    }
}
