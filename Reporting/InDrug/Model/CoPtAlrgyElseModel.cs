using Entity.Tenant;

namespace Reporting.InDrug.Model
{
    public class CoPtAlrgyElseModel
    {
        public PtAlrgyElse PtAlrgyElse { get; } = new();

        public CoPtAlrgyElseModel(PtAlrgyElse ptAlrgyElse)
        {
            PtAlrgyElse = ptAlrgyElse;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return PtAlrgyElse.HpId; }
        }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        public long PtId
        {
            get { return PtAlrgyElse.PtId; }
        }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        public int SeqNo
        {
            get { return PtAlrgyElse.SeqNo; }
        }

        /// <summary>
        /// 並び順
        /// 
        /// </summary>
        public int SortNo
        {
            get { return PtAlrgyElse.SortNo; }
        }

        /// <summary>
        /// アレルギー名称
        /// 
        /// </summary>
        public string AlrgyName
        {
            get { return PtAlrgyElse.AlrgyName ?? string.Empty; }
        }

        /// <summary>
        /// 発症日
        /// yyyymmdd or yyymm
        /// </summary>
        public int StartDate
        {
            get { return PtAlrgyElse.StartDate; }
        }

        /// <summary>
        /// 消失日
        /// yyyymmdd or yyymm
        /// </summary>
        public int EndDate
        {
            get { return PtAlrgyElse.EndDate; }
        }

        /// <summary>
        /// コメント
        /// 
        /// </summary>
        public string Cmt
        {
            get { return PtAlrgyElse.Cmt ?? string.Empty; }
        }

        /// <summary>
        /// 削除区分
        /// 1:削除
        /// </summary>
        public int IsDeleted
        {
            get { return PtAlrgyElse.IsDeleted; }
        }
    }
}
