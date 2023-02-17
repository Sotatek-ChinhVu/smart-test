namespace EmrCloudApi.Responses.InsuranceMst
{
    public class GetInfoCloneInsuranceMstResponse
    {
        public GetInfoCloneInsuranceMstResponse(int hokenEdaNo, int sortNo)
        {
            HokenEdaNo = hokenEdaNo;
            SortNo = sortNo;
        }

        public int HokenEdaNo { get; private set; }

        public int SortNo { get; private set; }
    }
}
