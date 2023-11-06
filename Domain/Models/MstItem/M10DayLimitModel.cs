using Helper.Common;
using Helper.Extension;

namespace Domain.Models.MstItem
{
    public class M10DayLimitModel
    {
        public M10DayLimitModel(string yjCd, int seqNo, int limitDay, string stDate, string edDate, string cmt)
        {
            YjCd = yjCd;
            SeqNo = seqNo;
            LimitDay = limitDay;
            StDate = stDate;
            EdDate = edDate;
            Cmt = cmt;
        }

        public M10DayLimitModel()
        {
            Cmt = string.Empty;
            YjCd = string.Empty;
            StDate = string.Empty;
            EdDate = string.Empty;
        }

        /// <summary>
        /// 個別医薬品コード
        /// YJコード
        /// </summary>
        public string YjCd { get; private set; }

        /// <summary>
        /// 連番
        /// 1..99
        /// </summary>
        public int SeqNo { get; private set; }

        /// <summary>
        /// 制限日数
        /// 1..999: 制限日数 (Null or 999 は制限日数なし)
        /// </summary>
        public int LimitDay { get; private set; }

        public string LimitDayBinding
        {
            get => CheckDefaultValue() ? string.Empty : LimitDay.AsString();
        }

        /// <summary>
        /// 適用開始日
        /// Null: 制限日数なし
        /// </summary>
        public string StDate { get; private set; }

        public string StartDateBinding
        {
            get => CheckDefaultValue() ? string.Empty : CIUtil.SDateToShowSDate(StDate.AsInteger());
        }

        /// <summary>
        /// 適用終了日
        /// Null: 連番が最大レコードの場合
        /// </summary>
        public string EdDate { get; private set; }

        public string EndDateBinding
        {
            get => CheckDefaultValue() ? string.Empty : (EdDate.AsInteger() == 99999999 ? "9999/99/99" : CIUtil.SDateToShowSDate(EdDate.AsInteger()));
        }

        /// <summary>
        /// コメント
        /// 特記事項があるときのみ記載
        /// </summary>
        public string Cmt { get; private set; }

        public bool CheckDefaultValue()
        {
            return LimitDay == 0 && string.IsNullOrEmpty(StDate) && string.IsNullOrEmpty(EdDate);
        }
    }
}
