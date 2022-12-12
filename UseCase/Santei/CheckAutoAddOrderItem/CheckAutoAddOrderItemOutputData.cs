using Domain.Models.Santei;
using UseCase.Core.Sync.Core;

namespace UseCase.Santei.CheckAutoAddOrderItem
{
    public class CheckAutoAddOrderItemOutputData : IOutputData
    {
        public CheckAutoAddOrderItemOutputData(List<SanteiOrderDetailModel> santeiOrderDetailModels, CheckAutoAddOrderItemStatus status)
        {
            SanteiOrderDetailModels = santeiOrderDetailModels;
            Status = status;
        }

        public List<SanteiOrderDetailModel> SanteiOrderDetailModels { get; private set; }
        public CheckAutoAddOrderItemStatus Status { get; private set; }
    }
}
