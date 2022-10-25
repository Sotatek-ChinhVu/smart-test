using Domain.Models.TodayOdr;

namespace EmrCloudApi.Tenant.Responses.MedicalExamination
{
    public class CheckedSpecialItemResponse
    {
        public CheckedSpecialItemResponse(List<CheckedSpecialItemModel> checkSpecialItemModels)
        {
            CheckSpecialItemModels = checkSpecialItemModels;
        }

        public List<CheckedSpecialItemModel> CheckSpecialItemModels { get; private set; }
    }
}
