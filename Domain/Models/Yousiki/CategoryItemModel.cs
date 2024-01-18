using Helper.Constants;
using Helper.Enum;

namespace Domain.Models.Yousiki
{
    public class CategoryItemModel
    {
        public CategoryItemModel(string categoryDisplayName, CategoryItemEnums categoryItemEnums, bool visibility, ModelStatus status, int dataType)
        {
            CategoryDisplayName = categoryDisplayName;
            CategoryItemEnums = categoryItemEnums;
            Visibility = visibility;
            Status = status;
            DataType = dataType;
        }

        public string CategoryDisplayName { get; private set; }

        public CategoryItemEnums CategoryItemEnums { get; private set; }

        public bool Visibility { get; private set; }

        public ModelStatus Status { get; private set; }

        public int DataType { get; private set; }
    }
}
