using Helper.Constants;

namespace Reporting.InDrug.Model
{
    public class CoInDrugModel
    {
        public CoPtInfModel PtInfModel;

        // 来院情報
        public CoRaiinInfModel RaiinInfModel;

        // オーダー情報
        public List<CoOdrInfModel> OdrInfModels;

        // オーダー情報詳細
        public List<CoOdrInfDetailModel> OdrInfDetailModels;

        // オーダー情報
        public List<CoOdrInfModel> OdrInfSyohosenCommentModels;

        // オーダー情報詳細
        public List<CoOdrInfDetailModel> OdrInfSyohosenCommentDetailModels;

        // オーダー情報
        public List<CoOdrInfModel> OdrInfSyohosenBikoModels;

        // オーダー情報詳細
        public List<CoOdrInfDetailModel> OdrInfSyohosenBikoDetailModels;

        public CoInDrugModel(
            CoPtInfModel ptInf, CoRaiinInfModel raiinInf,
            List<CoOdrInfModel> odrInfs, List<CoOdrInfDetailModel> odrDtls)
        {
            PtInfModel = ptInf;
            RaiinInfModel = raiinInf;
            OdrInfModels = odrInfs.FindAll(p => p.OdrKouiKbn < 100);
            OdrInfDetailModels = odrDtls.FindAll(p => p.OdrKouiKbn < 100);
            OdrInfSyohosenCommentModels = odrInfs.FindAll(p => p.OdrKouiKbn == 100);
            OdrInfSyohosenCommentDetailModels = odrDtls.FindAll(p => p.OdrKouiKbn == 100);
            OdrInfSyohosenBikoModels = odrInfs.FindAll(p => p.OdrKouiKbn == 101);
            OdrInfSyohosenBikoDetailModels = odrDtls.FindAll(p => p.OdrKouiKbn == 101);
        }

        public CoInDrugModel()
        {
            PtInfModel = new();
            RaiinInfModel = new();
            OdrInfModels = new();
            OdrInfDetailModels = new();
            OdrInfSyohosenCommentModels = new();
            OdrInfSyohosenCommentDetailModels = new();
            OdrInfSyohosenBikoModels = new();
            OdrInfSyohosenBikoDetailModels = new();
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
        /// <summary>
        /// 診療科名
        /// </summary>
        public string KaName
        {
            get => RaiinInfModel != new CoRaiinInfModel() ? RaiinInfModel.KaName : "";
        }
        /// <summary>
        /// 担当医氏名
        /// </summary>
        public string TantoName
        {
            get => RaiinInfModel != new CoRaiinInfModel() ? RaiinInfModel.TantoName : "";
        }
        /// <summary>
        /// 受付番号
        /// </summary>
        public int UketukeNo
        {
            get => RaiinInfModel != new CoRaiinInfModel() ? RaiinInfModel.UketukeNo : 0;
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
        public List<string> SyohosenComment
        {
            get
            {
                List<string> results = new List<string>();
                foreach (CoOdrInfDetailModel dtl in OdrInfSyohosenCommentDetailModels.FindAll(p => p.ItemCd != ItemCdConst.Con_Refill))
                {
                    results.Add(dtl.ItemName);
                }

                return results;
            }
        }
        public List<string> SyohosenBiko
        {
            get
            {
                List<string> results = new List<string>();
                foreach (CoOdrInfDetailModel dtl in OdrInfSyohosenBikoDetailModels.FindAll(p => p.ItemCd != ItemCdConst.Con_Refill))
                {
                    results.Add(dtl.ItemName);
                }

                return results;

            }
        }
    }
}
