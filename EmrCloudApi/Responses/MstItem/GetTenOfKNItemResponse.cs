namespace EmrCloudApi.Responses.MstItem
{
    public class GetTenOfKNItemResponse
    {
        public GetTenOfKNItemResponse(double latestSedai)
        {
            LatestSedai = latestSedai;
        }

        public double LatestSedai { get; private set; }
    }
}
