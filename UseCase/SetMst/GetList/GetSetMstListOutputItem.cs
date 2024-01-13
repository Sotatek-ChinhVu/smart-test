using System.Text.Json.Serialization;

namespace UseCase.SetMst.GetList
{
    public class GetSetMstListOutputItem
    {
        public GetSetMstListOutputItem(int hpId, int setCd, int setKbn, int setKbnEdaNo, int generationId, int level1, int level2, int level3, string setName, int weightKbn, int color, int isGroup, int isDelete, List<GetSetMstListOutputItem> childrens)
        {
            HpId = hpId;
            SetCd = setCd;
            SetKbn = setKbn;
            SetKbnEdaNo = setKbnEdaNo;
            GenerationId = generationId;
            Level1 = level1;
            Level2 = level2;
            Level3 = level3;
            SetName = setName;
            WeightKbn = weightKbn;
            Color = color;
            IsGroup = isGroup;
            IsDeleted = isDelete;
            Childrens = childrens;
        }

        [JsonPropertyName("hpId")]
        public int HpId { get; private set; }

        [JsonPropertyName("setCd")]
        public int SetCd { get; private set; }

        [JsonPropertyName("setKbn")]
        public int SetKbn { get; private set; }

        [JsonPropertyName("setKbnEdaNo")]
        public int SetKbnEdaNo { get; private set; }

        [JsonPropertyName("generationId")]
        public int GenerationId { get; private set; }

        [JsonPropertyName("level1")]
        public int Level1 { get; private set; }

        [JsonPropertyName("level2")]
        public int Level2 { get; private set; }

        [JsonPropertyName("level3")]
        public int Level3 { get; private set; }

        [JsonPropertyName("setName")]
        public string SetName { get; private set; }

        [JsonPropertyName("weightKbn")]
        public int WeightKbn { get; private set; }

        [JsonPropertyName("color")]
        public int Color { get; private set; }

        [JsonPropertyName("isGroup")]
        public int IsGroup { get; private set; }

        [JsonPropertyName("isDeleted")]
        public int IsDeleted { get; private set; }

        [JsonPropertyName("childrens")]
        public List<GetSetMstListOutputItem> Childrens { get; private set; }
    }
}
