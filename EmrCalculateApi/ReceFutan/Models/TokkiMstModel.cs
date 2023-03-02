using Entity.Tenant;

namespace EmrCalculateApi.ReceFutan.Models
{
    public class TokkiMstModel
    {
        public TokkiMst TokkiMst { get; }

        public TokkiMstModel(TokkiMst tokkiMst)
        {
            TokkiMst = tokkiMst;
        }

        /// <summary>
        /// 特記事項コード
        /// </summary>
        public string TokkiCd
        {
            get { return TokkiMst.TokkiCd; }
        }

        /// <summary>
        /// 特記事項名
        /// </summary>
        public string TokkiName
        {
            get { return TokkiMst.TokkiName; }
        }

        /// <summary>
        /// 使用開始日
        /// </summary>
        public int StartDate
        {
            get { return TokkiMst.StartDate; }
        }

        /// <summary>
        /// 使用終了日
        /// </summary>
        public int EndDate
        {
            get { return TokkiMst.EndDate; }
        }

    }

}
