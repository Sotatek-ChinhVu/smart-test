using Entity.Tenant;

namespace EmrCalculateApi.Futan.Models
{
    public class SinKouiModel
    {
        public SinKoui SinKoui { get; }

        public SinKouiModel(SinKoui sinKoui)
        {
            SinKoui = sinKoui;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return SinKoui.HpId; }
        }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        public long PtId
        {
            get { return SinKoui.PtId; }
        }

        /// <summary>
        /// 診療年月
        /// 
        /// </summary>
        public int SinYm
        {
            get { return SinKoui.SinYm; }
        }

        /// <summary>
        /// 剤番号
        /// SIN_RP_INF.RP_NO
        /// </summary>
        public int RpNo
        {
            get { return SinKoui.RpNo; }
        }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        public int SeqNo
        {
            get { return SinKoui.SeqNo; }
        }

        /// <summary>
        /// 保険組合せID
        /// 
        /// </summary>
        public int HokenPid
        {
            get { return SinKoui.HokenPid; }
        }

        /// <summary>
        /// 保険ID
        /// </summary>
        public int HokenId
        {
            get { return SinKoui.HokenId; }
        }

        /// <summary>
        /// 点数欄集計先
        /// TEN_MST.SYUKEI_SAKI + 枝番 ※別シート参照
        /// </summary>
        public string SyukeiSaki
        {
            get { return SinKoui.SyukeiSaki; }
        }

        /// <summary>
        /// 点数小計
        /// 
        /// </summary>
        public double Ten
        {
            get { return SinKoui.Ten; }
        }

        /// <summary>
        /// 消費税
        /// 
        /// </summary>
        public double Zei
        {
            get { return SinKoui.Zei; }
        }

        /// <summary>
        /// 回数小計
        /// 
        /// </summary>
        public int Count
        {
            get { return SinKoui.Count; }
        }

        /// <summary>
        /// 円点区分
        /// 0:点数 1:金額
        /// </summary>
        public int EntenKbn
        {
            get { return SinKoui.EntenKbn; }
        }

        /// <summary>
        /// コード区分
        /// 代表項目のTEN_MST.CD_KBN
        /// </summary>
        public string CdKbn
        {
            get { return SinKoui.CdKbn; }
        }

        /// <summary>
        /// 課税区分
        /// TEN_MST.KAZEI_KBN
        /// </summary>
        public int KazeiKbn
        {
            get { return SinKoui.KazeiKbn; }
        }

        /// <summary>
        /// 詳細インデックス
        /// 詳細をコード化したもの
        /// </summary>
        public string DetailData
        {
            get { return SinKoui.DetailData; }
        }

    }

}
