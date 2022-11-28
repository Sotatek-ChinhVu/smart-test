namespace UseCase.MedicalExamination.GetCheckDisease
{
    public class GetCheckDiseaseItemOutputData
    {
        public GetCheckDiseaseItemOutputData(string itemCd, string itemName, List<CheckedDiseaseItem> checkedDiseaseItems)
        {
            ItemCd = itemCd;
            ItemName = itemName;
            this.checkedDiseaseItems = checkedDiseaseItems;
        }

        public string ItemCd { get; private set; }

        public string ItemName { get; private set; }

        public List<CheckedDiseaseItem> checkedDiseaseItems { get; private set; }
    }
}
