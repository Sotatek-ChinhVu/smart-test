namespace Domain.Models.MstItem
{
    public class DosageMstModel
    {
        public DosageMstModel(int id, int hpId, string itemCd, int seqNo, double onceMin, double onceMax, double onceLimit, int onceUnit, double dayMin, double dayMax, double dayLimit, int dayUnit, int isDeleted, bool modelModified)
        {
            Id = id;
            HpId = hpId;
            ItemCd = itemCd;
            SeqNo = seqNo;
            OnceMin = onceMin;
            OnceMax = onceMax;
            OnceLimit = onceLimit;
            OnceUnit = onceUnit;
            DayMin = dayMin;
            DayMax = dayMax;
            DayLimit = dayLimit;
            DayUnit = dayUnit;
            IsDeleted = isDeleted;
            ModelModified = modelModified;
        }

        public DosageMstModel(int hpId , string itemCd)
        {
            HpId = hpId;
            ItemCd = itemCd;
        }

        public bool CheckDefaultValue()
        {
            return
                OnceMin == 0 &&
                OnceMax == 0 &&
                OnceLimit == 0 &&
                DayMin == 0 &&
                DayMax == 0 &&
                DayLimit == 0;
        }

        public int Id { get; private set; }

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
        /// 連番
        /// 
        /// </summary>
        public int SeqNo { get; private set; }

        /// <summary>
        /// 一回量最小値
        /// Y 薬価単位を基準にした一回当たりの最小投与量
        /// </summary>
        public double OnceMin { get; private set; }

        /// <summary>
        /// 一回量最大値
        /// Y 薬価単位を基準にした一回当たりの最大投与量
        /// </summary>
        public double OnceMax { get; private set; }

        /// <summary>
        /// 一回量上限値
        /// Y 薬価単位を基準にした一回当たりの上限投与量
        /// </summary>
        public double OnceLimit { get; private set; }

        /// <summary>
        /// 一回量単位
        /// 0:体重(体表面積)条件なし 1:/kg 2:/m2
        /// </summary>
        public int OnceUnit { get; private set; }

        /// <summary>
        /// 一日量最小値
        /// Y 薬価単位を基準にした一日当たりの最小投与量
        /// </summary>
        public double DayMin { get; private set; }

        /// <summary>
        /// 一日量最大値
        /// Y 薬価単位を基準にした一日当たりの最大投与量
        /// </summary>
        public double DayMax { get; private set; }

        /// <summary>
        /// 一日量上限値
        /// Y 薬価単位を基準にした一日当たりの上限投与量
        /// </summary>
        public double DayLimit { get; private set; }

        /// <summary>
        /// 一日量単位
        /// 0:体重(体表面積)条件なし 1:/kg 2:/m2
        /// </summary>
        public int DayUnit { get; private set; }

        /// <summary>
        /// 削除区分
        /// 1:削除
        /// </summary>
        public int IsDeleted { get; private set; }

        public bool ModelModified { get; private set; }
    }
}
