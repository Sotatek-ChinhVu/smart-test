using UseCase.Core.Sync.Core;

namespace UseCase.DrugDetailData.ShowProductInf
{
    public class ShowProductInfInputData : IInputData<ShowProductInfOutputData>
    {
        public ShowProductInfInputData(int hpId, int sinDate, string itemCd, int level, string drugName, string yJCode)
        {
            HpId = hpId;
            SinDate = sinDate;
            ItemCd = itemCd;
            Level = level;
            DrugName = drugName;
            YJCode = yJCode;
        }

        public int HpId { get; private set; }

        public int SinDate { get; private set; }

        public string ItemCd { get; private set; }

        public int Level { get; private set; }

        public string DrugName { get; private set; }

        public string YJCode { get; private set; }
    }
}
