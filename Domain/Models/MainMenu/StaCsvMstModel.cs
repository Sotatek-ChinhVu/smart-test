using System.Text.Json.Serialization;

namespace Domain.Models.MainMenu
{
    public class StaCsvMstModel
    {
        [JsonConstructor]
        public StaCsvMstModel(string groupName, bool isDefault, int dataSbt, int rowNo, bool isDeleted, List<StaCsvModel> staCsvModels, List<StaCsvModel> staCsvModelsSelected)
        {
            GroupName = groupName;
            IsDefault = isDefault;
            DataSbt = dataSbt;
            RowNo = rowNo;
            IsDeleted = isDeleted;
            StaCsvModels = staCsvModels;
            StaCsvModelsSelected = staCsvModelsSelected;
        }

        public StaCsvMstModel(string groupName, int dataSbt, List<StaCsvModel> staCsvModels, List<StaCsvModel> staCsvModelsSelected, int rowNo, bool isDefault)
        {
            GroupName = groupName;
            DataSbt = dataSbt;
            StaCsvModels = staCsvModels;
            StaCsvModelsSelected = staCsvModelsSelected;
            RowNo = rowNo;
            IsDefault = isDefault;
        }

        public string GroupName { get; private set; }

        public bool IsDefault { get; private set; }

        public int DataSbt { get; private set; }

        public int RowNo { get; private set; }

        public bool IsDeleted { get; private set; }

        public List<StaCsvModel> StaCsvModels { get; private set; }

        public List<StaCsvModel> StaCsvModelsSelected { get; private set; }
    }
}
