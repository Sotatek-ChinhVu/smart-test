using Helper.Common;

namespace Domain.Models.Online
{
    public class QualificationInfModel
    {
        public QualificationInfModel(string receptionNo, DateTime receptionDateTime, int yoyakuDate, string segmentOfResult, string errorMessage)
        {
            ReceptionNo = receptionNo;
            ReceptionDateTime = receptionDateTime;
            YoyakuDate = yoyakuDate;
            SegmentOfResult = segmentOfResult;
            ErrorMessage = errorMessage;
        }

        public QualificationInfModel()
        {
            ReceptionNo = string.Empty;
            SegmentOfResult = string.Empty;
            ErrorMessage = string.Empty;
        }

        public QualificationInfModel(string receptionNo, string segmentOfResult, string errorMessage)
        {
            ReceptionNo = receptionNo;
            SegmentOfResult = segmentOfResult;
            ErrorMessage = errorMessage;
        }

        public string ReceptionNo { get; private set; }

        public DateTime ReceptionDateTime { get; private set; }

        public int YoyakuDate { get; private set; }

        public string SegmentOfResult { get; private set; }

        public string ErrorMessage { get; private set; }

        #region Binding property

        public string YoyakuDateDisplay
        {
            get
            {
                if (YoyakuDate <= 0)
                {
                    return string.Empty;
                }
                return CIUtil.SDateToShowSDate(YoyakuDate);
            }
        }

        public string ReceptionDateTimeDisplay
        {
            get
            {
                if (CheckDefaultValue())
                {
                    return string.Empty;
                }
                return CIUtil.GetCIDateTimeStr(ReceptionDateTime, true);
            }
        }

        // 0 or 空:未処理、1:正常終了、2:処理中、9:異常終了
        public string SegmentOfResultDisplay
        {
            get
            {
                if (CheckDefaultValue())
                {
                    return string.Empty;
                }
                if (string.IsNullOrEmpty(SegmentOfResult) || SegmentOfResult == "0")
                {
                    return "照会中";
                }
                switch (SegmentOfResult)
                {
                    case "1":
                        return "正常終了";
                    case "2":
                        return "処理中";
                    case "9":
                        return "異常終了";
                }
                return string.Empty;
            }
        }
        #endregion

        public bool CheckDefaultValue()
        {
            return string.IsNullOrEmpty(ReceptionNo);
        }
    }
}
