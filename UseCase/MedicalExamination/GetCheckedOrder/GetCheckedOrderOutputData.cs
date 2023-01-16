using Domain.Models.MedicalExamination;
using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.GetCheckedOrder
{
    public class GetCheckedOrderOutputData : IOutputData
    {
        public GetCheckedOrderOutputData(GetCheckedOrderStatus status, List<CheckedOrderModel> checkedOrderModels)
        {
            Status = status;
            CheckedOrderModels = checkedOrderModels;
        }

        public GetCheckedOrderStatus Status { get; private set; }
        public List<CheckedOrderModel> CheckedOrderModels { get; private set; }
    }
}
