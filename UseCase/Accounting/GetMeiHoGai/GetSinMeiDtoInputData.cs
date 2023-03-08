namespace UseCase.Accounting.GetMeiHoGai
{
    public class GetSinMeiDtoInputData
    {
        public GetSinMeiDtoInputData(List<long> raiinNoList, long ptId, int sinDate, int hpId)
        {
            RaiinNoList = raiinNoList;
            PtId = ptId;
            SinDate = sinDate;
            HpId = hpId;
        }

        public List<long> RaiinNoList { get; private set; }
        public long PtId { get; private set; }
        public int SinDate { get; private set; }
        public int HpId { get; private set; }
    }
}
