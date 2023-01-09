using Entity.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmrCalculateApi.Ika.Models
{
    public class OdrInfCmtModel
    {
        public OdrInfCmt OdrInfCmt { get; } = null;

        public OdrInfCmtModel(OdrInfCmt odrInfCmt)
        {
            OdrInfCmt = odrInfCmt;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return OdrInfCmt.HpId; }
        }

        /// <summary>
        /// 診療日
        /// yyyymmdd
        /// </summary>
        public int SinDate
        {
            get { return OdrInfCmt.SinDate; }
        }

        /// <summary>
        /// 来院番号
        /// 
        /// </summary>
        public long RaiinNo
        {
            get { return OdrInfCmt.RaiinNo; }
        }

        /// <summary>
        /// 患者ID
        /// 患者を識別するためのシステム固有の番号
        /// </summary>
        public long PtId
        {
            get { return OdrInfCmt.PtId; }
        }

        /// <summary>
        /// 剤番号
        /// ODR_INF_DETAIL.RP_NO
        /// </summary>
        public long RpNo
        {
            get { return OdrInfCmt.RpNo; }
        }

        /// <summary>
        /// 剤枝番
        /// ODR_INF_DETAIL.RP_EDA_NO
        /// </summary>
        public long RpEdaNo
        {
            get { return OdrInfCmt.RpEdaNo; }
        }

        /// <summary>
        /// 行番号
        /// ODR_INF_DETAIL.ROW_NO
        /// </summary>
        public int RowNo
        {
            get { return OdrInfCmt.RowNo; }
        }

        /// <summary>
        /// 枝番
        /// ※2018/11/29現在、1項目につき、最大3つまで
        /// </summary>
        public int EdaNo
        {
            get { return OdrInfCmt.EdaNo; }
        }

        /// <summary>
        /// 並び順
        /// 
        /// </summary>
        public int SortNo
        {
            get { return OdrInfCmt.SortNo; }
        }

        /// <summary>
        /// 文字色
        /// 
        /// </summary>
        public int FontColor
        {
            get { return OdrInfCmt.FontColor; }
        }

        /// <summary>
        /// コメントコード
        /// 当該診療行為に対するコメントコード
        /// </summary>
        public string CmtCd
        {
            get { return OdrInfCmt.CmtCd ?? string.Empty; }
        }

        /// <summary>
        /// コメント名称
        /// コメントコードの名称
        /// </summary>
        public string CmtName
        {
            get { return OdrInfCmt.CmtName ?? string.Empty; }
        }

        /// <summary>
        /// コメント文
        /// コメントコードの定型文に組み合わせる文字情報
        /// </summary>
        public string CmtOpt
        {
            get { return OdrInfCmt.CmtOpt ?? string.Empty; }
        }


    }

}
