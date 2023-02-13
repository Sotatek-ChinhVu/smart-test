namespace EmrCloudApi.Requests.InsuranceMst
{
    public class DeleteHokenMasterRequest
    {
        public DeleteHokenMasterRequest(int prefNo, int hokenNo, int hokenEdaNo, int startDate)
        {
            PrefNo = prefNo;
            HokenNo = hokenNo;
            HokenEdaNo = hokenEdaNo;
            StartDate = startDate;
        }

        public int PrefNo { get; private set; }

        public int HokenNo { get; private set; }

        public int HokenEdaNo { get; private set; }

        public int StartDate { get; private set; }
    }
}
