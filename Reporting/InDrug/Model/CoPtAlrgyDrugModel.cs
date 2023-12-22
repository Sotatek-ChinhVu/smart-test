using Entity.Tenant;

namespace Reporting.InDrug.Model
{
    public class CoPtAlrgyDrugModel
    {
        public PtAlrgyDrug PtAlrgyDrug { get; } = new();

        public CoPtAlrgyDrugModel(PtAlrgyDrug ptAlrgyDrug)
        {
            PtAlrgyDrug = ptAlrgyDrug;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return PtAlrgyDrug.HpId; }
        }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        public long PtId
        {
            get { return PtAlrgyDrug.PtId; }
        }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        public int SeqNo
        {
            get { return PtAlrgyDrug.SeqNo; }
        }

        /// <summary>
        /// 並び順
        /// 
        /// </summary>
        public int SortNo
        {
            get { return PtAlrgyDrug.SortNo; }
        }

        /// <summary>
        /// 項目コード
        /// 
        /// </summary>
        public string ItemCd
        {
            get { return PtAlrgyDrug.ItemCd ?? string.Empty; }
        }

        /// <summary>
        /// 医薬品名称
        /// 
        /// </summary>
        public string DrugName
        {
            get { return PtAlrgyDrug.DrugName ?? ""; }
        }

        /// <summary>
        /// 発症日
        /// yyyymmdd or yyyymm
        /// </summary>
        public int StartDate
        {
            get { return PtAlrgyDrug.StartDate; }
        }

        /// <summary>
        /// 消失日
        /// yyyymmdd or yyyymm
        /// </summary>
        public int EndDate
        {
            get { return PtAlrgyDrug.EndDate; }
        }

        /// <summary>
        /// コメント
        /// 
        /// </summary>
        public string Cmt
        {
            get { return PtAlrgyDrug.Cmt ?? string.Empty; }
        }

        /// <summary>
        /// 削除区分
        /// 1:削除
        /// </summary>
        public int IsDeleted
        {
            get { return PtAlrgyDrug.IsDeleted; }
        }
    }
}
