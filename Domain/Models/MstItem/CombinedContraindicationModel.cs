namespace Domain.Models.MstItem
{
    public class CombinedContraindicationModel
    {
        public CombinedContraindicationModel(long id, int hpId, string aCd, string bCd, int seqNo, bool isDeleted, string name, bool isAddNew, bool isUpdated, string originBCd)
        {
            Id = id;
            HpId = hpId;
            ACd = aCd;
            BCd = bCd;
            SeqNo = seqNo;
            IsDeleted = isDeleted;
            Name = name;
            IsAddNew = isAddNew;
            IsUpdated = isUpdated;
            OriginBCd = originBCd;
        }

        public CombinedContraindicationModel()
        {
            ACd = string.Empty;
            BCd = string.Empty;
            Name = string.Empty;
            OriginBCd = string.Empty;
        }

        public long Id { get; private set; }
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId { get; private set; }

        /// <summary>
        /// Aコード
        /// TEN_MST.ITEM_CD
        /// </summary>
        public string ACd { get; private set; }

        /// <summary>
        /// Bコード
        /// TEN_MST.ITEM_CD
        /// </summary>
        public string BCd { get; private set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        public int SeqNo { get; private set; }

        /// <summary>
        /// 削除区分
        /// 1:削除
        /// </summary>
        public bool IsDeleted { get; private set; }

        
        public string Name { get; private set; }


        public bool IsAddNew { get; private set; }


        public bool IsUpdated { get; private set; }

        public string OriginBCd { get; set; } = string.Empty;

        public bool CheckDefaultValue()
        {
            return string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(BCd);
        }
    }
}
