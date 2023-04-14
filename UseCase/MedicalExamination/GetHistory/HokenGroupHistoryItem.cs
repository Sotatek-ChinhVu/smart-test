using System.Text.Json.Serialization;

namespace UseCase.MedicalExamination.GetHistory
{
    public class HokenGroupHistoryItem
    {
        public HokenGroupHistoryItem(int hokenPid, string hokenTitle, List<GroupOdrGHistoryItem> groupOdrItems)
        {
            HokenPid = hokenPid;
            HokenTitle = hokenTitle;
            GroupOdrItems = groupOdrItems;
        }

        [JsonPropertyName("hokenPid")]
        public int HokenPid { get; private set; }

        [JsonPropertyName("hokenTitle")]
        public string HokenTitle { get; private set; }

        [JsonPropertyName("groupOdrItems")]
        public List<GroupOdrGHistoryItem> GroupOdrItems { get; private set; }
    }
}
