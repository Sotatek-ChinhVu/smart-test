using Entity.Tenant;

namespace Reporting.Kensalrai.Model
{
    public class CoRaiinInfModel
    {
        RaiinInf RaiinInf { get; } = null;
        KaMst KaMst { get; } = null;
        UserMst UserMst { get; } = null;
        public CoRaiinInfModel(RaiinInf raiinInf, KaMst kaMst, UserMst userMst)
        {
            RaiinInf = raiinInf;
            KaMst = kaMst;
            UserMst = userMst;
        }
        /// <summary>
        /// 来院番号
        /// </summary>
        public long RaiinNo
        {
            get { return RaiinInf.RaiinNo; }
        }
        /// <summary>
        /// 診療科名称
        /// </summary>
        public string KaName
        {
            get { return KaMst != null ? KaMst.KaName : ""; }
        }
        /// <summary>
        /// 担当医氏名
        /// </summary>
        public string TantoName
        {
            get { return UserMst != null ? UserMst.Name : ""; }
        }
        /// <summary>
        /// 担当医カナ氏名
        /// </summary>
        public string TantoKanaName
        {
            get { return UserMst != null ? UserMst.KanaName : ""; }
        }
        public string DrName
        {
            get
            {
                string ret = "";

                if (UserMst != null)
                {
                    ret = TantoName;

                    if (string.IsNullOrEmpty(UserMst.DrName) == false)
                    {
                        ret = UserMst.DrName;
                    }
                }

                return ret;
            }
        }
    }
}
