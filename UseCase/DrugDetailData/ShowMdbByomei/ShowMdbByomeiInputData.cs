using UseCase.Core.Sync.Core;

namespace UseCase.DrugDetailData.ShowMdbByomei
{
    public class ShowMdbByomeiInputData : IInputData<ShowMdbByomeiOutputData>
    {
        public ShowMdbByomeiInputData(int hpId, string itemCd, int level, string drugName, string yJCode)
        {
            HpId = hpId;
            ItemCd = itemCd;
            Level = level;
            DrugName = drugName;
            YJCode = yJCode;
        }

        public int HpId { get; private set; }

        public string ItemCd { get; private set; }

        public int Level { get; private set; }

        public string DrugName { get; private set; }

        public string YJCode { get; private set; }
    }
}
