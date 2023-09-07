using Domain.Models.MstItem;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetSingleDoseMstAndMedicineUnitList
{
    public sealed class GetSingleDoseMstAndMedicineUnitListOutputData : IOutputData
    {
        public GetSingleDoseMstAndMedicineUnitListOutputData(bool status, List<SingleDoseMstModel> singleDoseMsts, List<MedicineUnitModel> medicineUnits)
        {
            Status = status;
            SingleDoseMsts = singleDoseMsts;
            MedicineUnits = medicineUnits;
        }
        public bool Status { get; private set; }
        public List<SingleDoseMstModel> SingleDoseMsts { get; private set; }
        public List<MedicineUnitModel> MedicineUnits { get; private set; }
    }
}
