using UseCase.MstItem.GetListDrugImage;

namespace EmrCloudApi.Responses.MstItem
{
    public class GetListDrugImageResponse
    {
        public GetListDrugImageResponse(List<DrugImageOutputItem> imageList)
        {
            ImageLists = imageList;
        }

        public List<DrugImageOutputItem> ImageLists { get; private set; }
    }
}
