using Helper.Common;
using Helper.Extension;

namespace Domain.Models.MstItem
{
    public class DrugDayLimitModel
    {
        public DrugDayLimitModel(int id, int hpId, string itemCd, int seqNo, int limitDay, int startDate, int endDate, int isDeleted, bool modelModified)
        {
            Id = id;
            HpId = hpId;
            ItemCd = itemCd;
            SeqNo = seqNo;
            LimitDay = limitDay;
            StartDate = startDate;
            EndDate = endDate;
            IsDeleted = isDeleted;
            ModelModified = modelModified;
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
        /// 制限日数
        /// or 999:制限なし
        /// </summary>
        public int LimitDay { get; private set; }


        /// <summary>
        /// 新設年月日
        /// </summary>
        public int StartDate { get; private set; }

        /// <summary>
        /// 廃止年月日
        /// </summary>
        public int EndDate { get; private set; }

        /// <summary>
        /// 削除区分
        /// 1:削除
        /// </summary>
        public int IsDeleted { get; private set; }

        public bool ModelModified { get; private set; }

        public bool CheckDefaultValue()
        {
            return LimitDay == 0 && StartDate == 0 && EndDate == 99999999;
        }
    }
}
