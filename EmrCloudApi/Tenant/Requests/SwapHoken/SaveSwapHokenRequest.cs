namespace EmrCloudApi.Tenant.Requests.SwapHoken
{
    public class SaveSwapHokenRequest
    {
        public SaveSwapHokenRequest(long ptId, int hokenIdBefore, int hokenIdAfter, int hokenPidBefore, int hokenPidAfter, int startDate, int endDate)
        {
            PtId = ptId;
            HokenIdBefore = hokenIdBefore;
            HokenIdAfter = hokenIdAfter;
            HokenPidBefore = hokenPidBefore;
            HokenPidAfter = hokenPidAfter;
            StartDate = startDate;
            EndDate = endDate;
        }

        public long PtId { get; private set; }

        public int HokenIdBefore { get; private set; }

        public int HokenIdAfter { get; private set; }

        public int HokenPidBefore { get; private set; }

        public int HokenPidAfter { get; private set; }

        public int StartDate { get; private set; }

        public int EndDate { get; private set; }
    }
}