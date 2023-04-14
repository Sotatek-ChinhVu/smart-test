using Helper.Common;
using Helper.Constants;

namespace Reporting.Sijisen.Model
{
    public class CoSijisenModel
    {
        public CoPtInfModel PtInfModel;

        // 来院情報
        public CoRaiinInfModel RaiinInfModel;
        public List<CoRaiinInfModel> OtherRaiinInfModels;

        // オーダー情報
        public List<CoCommonOdrInfModel> OdrInfModels;

        // オーダー情報詳細
        public List<CoCommonOdrInfDetailModel> OdrInfDetailModels;
        // 予約オーダー情報
        public List<CoRsvkrtOdrInfModel> RsvkrtOdrInfModels;

        // 予約オーダー情報詳細
        public List<CoRsvkrtOdrInfDetailModel> RsvkrtOdrInfDetailModels;
        // 来院区分
        public List<CoRaiinKbnInfModel> RaiinKbnInfModels;

        public CoSijisenModel(
            CoPtInfModel ptInf, CoRaiinInfModel raiinInf,
        List<CoCommonOdrInfModel> odrInfs, List<CoCommonOdrInfDetailModel> odrDtls,
            List<CoRaiinKbnInfModel> raiinKbnInfs, List<CoRaiinInfModel> otherRaiinInfs,
            int lastSinDate)
        {
            PtInfModel = ptInf;
            RaiinInfModel = raiinInf;
            OdrInfModels = odrInfs;
            OdrInfDetailModels = odrDtls;
            RaiinKbnInfModels = raiinKbnInfs;
            OtherRaiinInfModels = otherRaiinInfs;
            LastSinDate = lastSinDate;
            RsvkrtOdrInfModels = new();
            RsvkrtOdrInfDetailModels = new();
        }

        /// <summary>
        /// 患者番号
        /// </summary>
        public long PtNum
        {
            get => PtInfModel.PtNum;
        }
        /// <summary>
        /// 患者ID
        /// </summary>
        public long PtId
        {
            get => PtInfModel.PtId;
        }
        /// <summary>
        /// 患者氏名
        /// </summary>
        public string PtName
        {
            get => PtInfModel.Name ?? "";
        }
        /// <summary>
        /// 患者カナ氏名
        /// </summary>
        public string PtKanaName
        {
            get => PtInfModel.KanaName ?? "";
        }
        /// <summary>
        /// 性別
        /// </summary>
        public int Sex
        {
            get => PtInfModel.Sex;
        }

        /// <summary>
        /// 性別を"男", "女"で返す
        /// </summary>
        public string PtSex
        {
            get
            {
                string ret = "男";

                if (Sex == 2)
                {
                    ret = "女";
                }

                return ret;
            }
        }
        /// <summary>
        /// 生年月日
        /// </summary>
        public int BirthDay
        {
            get => PtInfModel.Birthday;
        }
        /// <summary>
        /// 年齢
        /// </summary>
        public int Age
        {
            get => PtInfModel.Age;
        }
        public string AgeDsp
        {
            get => PtInfModel.AgeDsp;
        }
        public string AgeDspMonth
        {
            get => PtInfModel.AgeDspMonth;
        }
        public string AgeDspDay
        {
            get => PtInfModel.AgeDspDay;
        }
        /// <summary>
        /// 患者郵便番号
        /// </summary>
        public string PtPostCd
        {
            get => PtInfModel.HomePost;
        }
        public string PtPostCdDsp
        {
            get => CIUtil.GetDspPostCd(PtInfModel.HomePost);
        }
        /// <summary>
        /// 患者住所
        /// </summary>
        public string PtAddress
        {
            get => PtInfModel.HomeAddress;
        }
        /// <summary>
        /// 患者電話番号
        /// </summary>
        public string PtTel
        {
            get => PtInfModel.Tel;
        }

        /// <summary>
        /// 受付番号
        /// </summary>
        public int UketukeNo
        {
            get => RaiinInfModel != null ? RaiinInfModel.UketukeNo : 0;
        }
        /// <summary>
        /// 受付種別名
        /// </summary>
        public string UketukeSbtName
        {
            get => RaiinInfModel != null ? RaiinInfModel.UketukeSbtName : "";
        }
        /// <summary>
        /// 受付種別名（同日他来院）
        /// </summary>
        public string UketukeSbtNameEtc
        {
            get
            {
                string ret = "";

                if (OtherRaiinInfModels != null && OtherRaiinInfModels.Any())
                {
                    foreach (CoRaiinInfModel otherRaiinInfModel in OtherRaiinInfModels)
                    {
                        if (otherRaiinInfModel.UketukeSbtName != UketukeSbtName && ret.Contains(otherRaiinInfModel.UketukeSbtName) == false)
                        {
                            if (ret != "") ret += ",";
                            ret = otherRaiinInfModel.UketukeSbtName;
                        }
                    }
                }

                return ret;
            }
        }
        /// <summary>
        /// 来院区分名
        /// </summary>
        /// <param name="grpId">分類kID</param>
        /// <returns>来院区分名</returns>
        public string GetRaiinKbnName(int grpId)
        {
            string ret = "";

            if (RaiinKbnInfModels != null && RaiinKbnInfModels.Any(p => p.GrpId == grpId))
            {
                ret = RaiinKbnInfModels.Find(p => p.GrpId == grpId)?.KbnName ?? string.Empty;
            }

            return ret;
        }

        /// <summary>
        /// 受付時間
        /// </summary>
        public string UketukeTime
        {
            get
            {
                string ret = RaiinInfModel != null ? (RaiinInfModel.UketukeTime ?? "") : "";

                if (ret != "")
                {
                    ret = ret.PadLeft(4, '0');
                    ret = ret.Substring(0, 2) + ":" + ret.Substring(2, 2);
                }
                return ret;
            }

        }

        /// <summary>
        /// 予約時間
        /// </summary>
        public string YoyakuTime
        {
            get
            {
                string ret = "";

                if (RaiinInfModel != null &&
                    (RaiinInfModel.Status == RaiinState.Reservation || (string.IsNullOrEmpty(RaiinInfModel.YoyakuTime) == false && RaiinInfModel.YoyakuTime != "0")))
                {
                    ret = RaiinInfModel.YoyakuTime;

                    if (string.IsNullOrEmpty(ret) == false)
                    {
                        ret = ret.PadLeft(4, '0');
                        ret = ret.Substring(0, 2) + ":" + ret.Substring(2, 2);
                    }
                }
                return ret;
            }

        }
        /// <summary>
        /// 診療科名
        /// </summary>
        public string KaName
        {
            get => RaiinInfModel != null ? RaiinInfModel.KaName : "";
        }
        /// <summary>
        /// 担当医氏名
        /// </summary>
        public string TantoName
        {
            get => RaiinInfModel != null ? RaiinInfModel.TantoName : "";
        }
        /// <summary>
        /// 来院コメント
        /// </summary>
        public string RaiinCmt
        {
            get => RaiinInfModel != null ? RaiinInfModel.RaiinCmt : "";
        }
        /// <summary>
        /// アレルギー情報
        /// </summary>
        public List<string> Alrgy
        {
            get
            {
                return PtInfModel.Alrgy;
            }
        }
        /// <summary>
        /// 患者コメント（改行区切り）
        /// </summary>
        public List<string> PtCmtList
        {
            get
            {
                List<string> ret = new List<string>();

                if (PtInfModel.PtCmt != "")
                {
                    string[] del = { "\r\n", "\r", "\n" };
                    string[] tmps = PtInfModel.PtCmt.Split(del, StringSplitOptions.None);

                    foreach (string tmp in tmps)
                    {
                        ret.Add(tmp);
                    }
                }

                return ret;
            }
        }
        /// <summary>
        /// 患者コメント
        /// </summary>
        public string PtCmt
        {
            get
            {
                return PtInfModel.PtCmt;
            }
        }
        /// <summary>
        /// 最終来院日
        /// </summary>
        public int LastSinDate { get; set; }
    }
}
