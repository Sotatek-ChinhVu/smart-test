using UseCase.Core.Sync.Core;

namespace UseCase.DrugDetailData.Get
{
    public class GetDrugDetailDataInputData : IInputData<GetDrugDetailDataOutputData>
    {
        public GetDrugDetailDataInputData(int selectedIndexOfMenuLelel, int level, string drugName, string itemCd, string yJCode)
        {
            SelectedIndexOfMenuLevel = selectedIndexOfMenuLelel;
            Level = level;
            DrugName = drugName;
            ItemCd = itemCd;
            YJCode = yJCode;
        }

        public int SelectedIndexOfMenuLevel { get; private set; }

        public int Level { get; private set; }

        public string DrugName { get; private set; }

        public string ItemCd { get; private set; }

        public string YJCode { get; private set; }
    }
}
