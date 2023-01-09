using Entity.Tenant;
using EmrCalculateApi.Ika.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmrCalculateApi.Receipt.Models
{
    public class ReceCmtModel
    {
        public ReceCmt ReceCmt { get; } = null;

        public ReceCmtModel(ReceCmt receCmt)
        {
            ReceCmt = receCmt;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return ReceCmt.HpId; }
        }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        public long PtId
        {
            get { return ReceCmt.PtId; }
        }

        /// <summary>
        /// 診療年月
        /// 
        /// </summary>
        public int SinYm
        {
            get { return ReceCmt.SinYm; }
        }

        /// <summary>
        /// 保険ID
        /// 
        /// </summary>
        public int HokenId
        {
            get { return ReceCmt.HokenId; }
        }

        /// <summary>
        /// コメント区分
        /// 1:ヘッダー 2:フッター
        /// </summary>
        public int CmtKbn
        {
            get { return ReceCmt.CmtKbn; }
        }

        /// <summary>
        /// コメント種別
        /// 0:コメント文（ITEM_CDあり）、1:フリーコメント
        /// </summary>
        public int CmtSbt
        {
            get { return ReceCmt.CmtSbt; }
        }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        public int SeqNo
        {
            get { return ReceCmt.SeqNo; }
        }

        /// <summary>
        /// コメントコード
        /// フリーコメントはNULL
        /// </summary>
        public string ItemCd
        {
            get { return ReceCmt.ItemCd ?? string.Empty; }
        }

        /// <summary>
        /// コメント
        /// 
        /// </summary>
        public string Cmt
        {
            get { return ReceCmt.Cmt ?? string.Empty; }
        }

        /// <summary>
        /// コメントデータ
        /// コメントマスターの定型文に組み合わせる文字情報
        /// </summary>
        public string CmtData
        {
            get { return ReceCmt.CmtData ?? string.Empty; }
        }

        /// <summary>
        /// 削除区分
        /// 1:削除
        /// </summary>
        public int IsDeleted
        {
            get { return ReceCmt.IsDeleted; }
        }
    }

}
