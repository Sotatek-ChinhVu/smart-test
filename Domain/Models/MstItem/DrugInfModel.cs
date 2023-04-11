namespace Domain.Models.MstItem
{
    public class DrugInfModel
    {
        public DrugInfModel(int hpId, string itemCd, int infKbn, long seqNo, string drugInfo, int isDeleted, bool isModified)
        {
            HpId = hpId;
            ItemCd = itemCd;
            InfKbn = infKbn;
            SeqNo = seqNo;
            DrugInfo = drugInfo;
            IsDeleted = isDeleted;
            IsModified = isModified;
        }


        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId { get; private set; }

        /// <summary>
        /// 項目コード
        /// 
        /// </summary>
        public string ItemCd { get; private set; }

        /// <summary>
        /// 情報区分
        /// 0:薬剤情報 1:薬の作用 2:注意事項
        /// </summary>
        public int InfKbn { get; private set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        public long SeqNo { get; private set; }

        /// <summary>
        /// 薬剤情報
        /// 
        /// </summary>
        public string DrugInfo { get; private set; }

        /// <summary>
        /// 削除区分
        /// 1:削除
        /// </summary>
        public int IsDeleted { get; private set; }

        

        public bool IsNewModel
        {
            get => HpId == 0;
        }

        public bool IsDefaultModel
        {
            get => IsNewModel && string.IsNullOrEmpty(DrugInfo);
        }

        
        public bool IsModified { get; private set; }
    }
}
