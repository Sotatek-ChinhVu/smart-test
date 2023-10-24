using Domain.Models.MstItem;

namespace EmrCloudApi.Responses.MstItem
{
    public class GetSingleDoseMstAndMedicineUnitResponse
    {
        public GetSingleDoseMstAndMedicineUnitResponse(List<SingleDoseMstModel> singleDoseMsts, List<MedicineUnitModel> medicineUnits)
        {
            SingleDoseMsts = singleDoseMsts;
            MedicineUnits = medicineUnits;
        }

        public List<SingleDoseMstModel> SingleDoseMsts { get; set; }
        public List<MedicineUnitModel> MedicineUnits { get; set; }
    }
}
