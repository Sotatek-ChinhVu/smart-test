using Entity.Tenant;

namespace EmrCalculateApi.Futan.Models
{
    public class PtSanteiConfModel
    {
        public PtSanteiConf PtSanteiConf { get; private set; }

        public PtSanteiConfModel(PtSanteiConf ptSanteiConf)
        {
            PtSanteiConf = ptSanteiConf;
        }

        /// <summary>
        /// 区分番号
        ///     1: 調整額
        ///     2: 調整率
        ///     3: 自動算定
        /// </summary>
        public int KbnNo
        {
            get { return PtSanteiConf.KbnNo; }
            set
            {
                if (PtSanteiConf.KbnNo == value) return;
                PtSanteiConf.KbnNo = value;
            }
        }

        /// <summary>
        /// 区分番号枝番
        ///     KBN_NO: 1: 調整額
        ///             (   0: すべて
        ///                 1: 自費除く 
        ///                 2: 自費のみ  )
        ///     KBN_NO: 2: 調整率
        ///             (   0: すべて
        ///                 1: 自費除く 
        ///                 2: 自費のみ  )
        ///     KBN_NO: 3: 自動算定
        ///             (   1: 地域包括診療料                 
        ///                 2: 認知症地域包括診療料 )
        /// </summary>
        public int EdaNo
        {
            get { return PtSanteiConf.EdaNo; }
            set
            {
                if (PtSanteiConf.EdaNo == value) return;
                PtSanteiConf.EdaNo = value;
            }
        }

        /// <summary>
        /// 区分値 
        /// </summary>
        public int KbnVal
        {
            get { return PtSanteiConf.KbnVal; }
            set
            {
                if (PtSanteiConf.KbnVal == value) return;
                PtSanteiConf.KbnVal = value;
            }
        }
    }
}
