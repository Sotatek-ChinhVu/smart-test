using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.GetContainerMst
{
    public class GetContainerMstInputData : IInputData<GetContainerMstOutputData>
    {
        public GetContainerMstInputData(int hpId, int sinDate, bool defaultChecked, List<OdrInfItem> odrInfItems)
        {
            HpId = hpId;
            SinDate = sinDate;
            DefaultChecked = defaultChecked;
            OdrInfItems = odrInfItems;
        }

        public int HpId { get; private set; }

        public int SinDate { get; private set; }

        public bool DefaultChecked { get; private set; }

        public List<OdrInfItem> OdrInfItems { get; private set; }
    }

    public class OdrInfItem
    {
        public OdrInfItem(int inoutKbn, int odrKouiKbn, int isDeleted, List<OdrInfDetailItem> odrInfDetailItems)
        {
            InoutKbn = inoutKbn;
            OdrKouiKbn = odrKouiKbn;
            IsDeleted = isDeleted;
            OdrInfDetailItems = odrInfDetailItems;
        }

        public int InoutKbn { get; private set; }

        public int OdrKouiKbn { get; private set; }

        public int IsDeleted { get; private set; }

        public List<OdrInfDetailItem> OdrInfDetailItems { get; private set; }
    }

    public class OdrInfDetailItem
    {
        public OdrInfDetailItem(string itemCd, string masterSbt)
        {
            ItemCd = itemCd;
            MasterSbt = masterSbt;
        }

        public string ItemCd { get; private set; }

        public string MasterSbt { get; private set; }
    }
}
