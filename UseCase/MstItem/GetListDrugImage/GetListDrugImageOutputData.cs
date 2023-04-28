using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetListDrugImage
{
    public class GetListDrugImageOutputData : IOutputData
    {
        public GetListDrugImageOutputData(GetListDrugImageStatus status, List<string> imageUrl)
        {
            Status = status;
            ImageUrl = imageUrl;
        }

        public GetListDrugImageStatus Status { get; private set; }

        public List<string> ImageUrl { get; private set; }
    }
}
