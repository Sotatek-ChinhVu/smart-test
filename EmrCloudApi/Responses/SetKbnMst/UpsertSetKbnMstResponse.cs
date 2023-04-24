namespace EmrCloudApi.Responses.SetKbnMst
{
    public class UpsertSetKbnMstResponse
    {
        public UpsertSetKbnMstResponse(bool value)
        {
            Value = value;
        }

        public bool Value { get; private set; }
    }
}