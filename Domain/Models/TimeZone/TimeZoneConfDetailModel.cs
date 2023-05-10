using Helper.Common;

namespace Domain.Models.TimeZone
{
    public class TimeZoneConfDetailModel
    {
        public TimeZoneConfDetailModel(int hpId, int sortNo, int youbiKbn, int startTime, int endTime, long seqNo, int timeKbn, int isDelete, bool modelModified)
        {
            HpId = hpId;
            SortNo = sortNo;
            YoubiKbn = youbiKbn;
            StartTime = startTime;
            EndTime = endTime;
            SeqNo = seqNo;
            TimeKbn = timeKbn;
            IsDelete = isDelete;
            ModelModified = modelModified;
            IsNewStartTime = false;
            IsNewEndTime = false;
        }

        /// <summary>
        /// 時間帯設定
        /// </summary>
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        public int HpId { get; private set; }

        public int SortNo { get; private set; }

        /// <summary>
        /// 曜日区分
        ///     1..7:日曜～土曜
        /// </summary>
        public int YoubiKbn { get; private set; }
        /// <summary>
        /// 開始時間
        /// </summary>
        public int StartTime { get; private set; }

        public string StartTimeBinding
        {
            get
            {
                if (StartTime == 0)
                {
                    if (IsNewStartTime) return "";
                    else return "00:00";
                }
                var str = CIUtil.TryCIToTimeZone(StartTime);
                return str == "00:00" ? "" : str;
            }
        }

        /// <summary>
        /// 終了時間
        /// </summary>
        public int EndTime { get; private set; }

        public string EndTimeBinding
        {
            get
            {
                if (EndTime == 0)
                {
                    if (IsNewEndTime) return "";
                    else return "00:00";
                }
                var str = CIUtil.TryCIToTimeZone(EndTime);
                return str == "00:00" ? "" : str;
            }
        }

        /// <summary>
        /// 連番
        /// </summary>
        public long SeqNo { get; private set; }

        /// <summary>
        /// 時間区分
        ///     0:時間内 ※時間内のレコードは不要
        ///     1:時間外
        ///     2:休日 ※HOLIDAY_MSTで設定するため未使用
        ///     3:深夜
        ///     4:夜間早朝
        /// </summary>
        public int TimeKbn { get; private set; }

        /// <summary>
        /// 削除区分
        ///     1:削除
        /// </summary>
        public int IsDelete { get; private set; }

        public bool ModelModified { get; private set; }

        public bool CheckDefaultValue()
        {
            return StartTime == 0 && EndTime == 0 && StartTimeBinding == "" && EndTimeBinding == "";
        }

        public bool IsDefaultValueItem
        {
            get => StartTime == 0 && EndTime == 0 || IsDelete == 1;
        }

        public bool IsNewStartTime { get; private set; }

        public bool IsNewEndTime { get; private set; }

        public void SetIsNewStartTime(bool value) => IsNewStartTime = value;

        public void SetIsNewEndTime(bool value) => IsNewEndTime = value;
    }
}
