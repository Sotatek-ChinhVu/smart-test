using UseCase.MedicalExamination.GetAddedAutoItem;

namespace EmrCloudApi.Responses.MedicalExamination
{
    public class GetAddedAutoItemResponse
    {
        public GetAddedAutoItemResponse(List<AddedAutoItem> addedAutoItems)
        {
            AddedAutoItems = addedAutoItems;
        }

        public List<AddedAutoItem> AddedAutoItems { get; private set; }
    }
}
