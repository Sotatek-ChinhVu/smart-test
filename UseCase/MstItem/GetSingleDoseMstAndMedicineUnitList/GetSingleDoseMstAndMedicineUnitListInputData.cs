using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetSingleDoseMstAndMedicineUnitList
{
    public sealed class GetSingleDoseMstAndMedicineUnitListInputData : IInputData<GetSingleDoseMstAndMedicineUnitListOutputData>
    {
        public GetSingleDoseMstAndMedicineUnitListInputData(int hpId)
        {
            HpId = hpId;
        }

        public int HpId { get; private set; }
    }
}
