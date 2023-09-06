using Domain.Models.MstItem;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetSingleDoseMstAndMedicineUnitList
{
    public sealed class GetSingleDoseMstAndMedicineUnitListOutputData : IOutputData
    {
        public GetSingleDoseMstAndMedicineUnitListOutputData(List<SingleDoseMstModel> singleDoseMsts, List<MedicineUnitModel> medicineUnits)
        {
            SingleDoseMsts = singleDoseMsts;
            MedicineUnits = medicineUnits;
        }

        public List<SingleDoseMstModel> SingleDoseMsts { get; private set; }
        public List<MedicineUnitModel> MedicineUnits { get; private set; }
    }
}
