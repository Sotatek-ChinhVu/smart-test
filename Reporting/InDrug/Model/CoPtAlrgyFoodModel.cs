using Entity.Tenant;

namespace Reporting.InDrug.Model
{
    public class CoPtAlrgyFoodModel
    {
        public PtAlrgyFood PtAlrgyFood { get; } = new();
        public M12FoodAlrgyKbn FoodAlrgyKbn { get; } = new();

        public CoPtAlrgyFoodModel(PtAlrgyFood ptAlrgyFood, M12FoodAlrgyKbn foodAlrgyKbn)
        {
            PtAlrgyFood = ptAlrgyFood;
            FoodAlrgyKbn = foodAlrgyKbn;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return PtAlrgyFood.HpId; }
        }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        public long PtId
        {
            get { return PtAlrgyFood.PtId; }
        }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        public int SeqNo
        {
            get { return PtAlrgyFood.SeqNo; }
        }

        /// <summary>
        /// 並び順
        /// 
        /// </summary>
        public int SortNo
        {
            get { return PtAlrgyFood.SortNo; }
        }

        /// <summary>
        /// アレルギー区分
        /// 
        /// </summary>
        public string AlrgyKbn
        {
            get { return PtAlrgyFood.AlrgyKbn ?? string.Empty; }
        }

        /// <summary>
        /// 発症日
        /// yyyymmdd or yyymm
        /// </summary>
        public int StartDate
        {
            get { return PtAlrgyFood.StartDate; }
        }

        /// <summary>
        /// 消失日
        /// yyyymmdd or yyymm
        /// </summary>
        public int EndDate
        {
            get { return PtAlrgyFood.EndDate; }
        }

        /// <summary>
        /// コメント
        /// 
        /// </summary>
        public string Cmt
        {
            get { return PtAlrgyFood.Cmt ?? string.Empty; }
        }

        /// <summary>
        /// 削除区分
        /// 1:削除
        /// </summary>
        public int IsDeleted
        {
            get { return PtAlrgyFood.IsDeleted; }
        }

        /// <summary>
        /// 食物名称
        /// </summary>
        public string FoodName
        {
            get { return FoodAlrgyKbn != new M12FoodAlrgyKbn() ? (FoodAlrgyKbn.FoodName ?? "") : ""; }
        }
    }
}
