namespace EmrCloudApi.Responses.MstItem
{
    public class GetListDrugImageResponse
    {
        public GetListDrugImageResponse(List<string> drugImages)
        {
            DrugImages = drugImages;
        }

        public List<string> DrugImages { get; private set; }
    }
}
