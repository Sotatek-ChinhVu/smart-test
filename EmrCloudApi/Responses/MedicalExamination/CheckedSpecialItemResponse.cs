using UseCase.OrdInfs.CheckedSpecialItem;

namespace EmrCloudApi.Responses.MedicalExamination
{
    public class CheckedSpecialItemResponse
    {
        public CheckedSpecialItemResponse(List<CheckedSpecialItem> checkSpecialItemModels)
        {
            CheckSpecialItemModels = checkSpecialItemModels;
        }

        public List<CheckedSpecialItem> CheckSpecialItemModels { get; private set; }
    }
}
