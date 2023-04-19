using UseCase.SetKbnMst.Upsert;

namespace EmrCloudApi.Requests.SetKbnMst
{
    public class UpsertSetKbnMstRequest
    {
        public UpsertSetKbnMstRequest(int sinDate, List<SetKbnMstItem> setKbnMstItems)
        {
            SinDate = sinDate;
            SetKbnMstItems = setKbnMstItems;
        }

        public int SinDate { get; set; }
        public List<SetKbnMstItem> SetKbnMstItems { get; set; }
    }
}