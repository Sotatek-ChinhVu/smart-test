namespace EmrCloudApi.Requests.TimeZoneConf
{
    public class TimeZoneConfDto
    {
        public TimeZoneConfDto(long seqNo, int timeKbn, int sortNo, int youbiKbn, int startTime, int endTime, int isDelete, bool modelModified)
        {
            SeqNo = seqNo;
            TimeKbn = timeKbn;
            SortNo = sortNo;
            YoubiKbn = youbiKbn;
            StartTime = startTime;
            EndTime = endTime;
            IsDelete = isDelete;
            ModelModified = modelModified;
        }

        public long SeqNo { get; private set; }

        public int TimeKbn { get; private set; }

        public int SortNo { get; private set; }

        public int YoubiKbn { get; private set; }

        public int StartTime { get; private set; }

        public int EndTime { get; private set; }

        public int IsDelete { get; private set; }

        public bool ModelModified { get; private set; }
    }
}
