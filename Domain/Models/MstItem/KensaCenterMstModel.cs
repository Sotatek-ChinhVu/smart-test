using Helper.Constants;

namespace Domain.Models.MstItem
{
    public class KensaCenterMstModel
    {
        public KensaCenterMstModel(long id, int hpId, string centerCd, string centerName, int primaryKbn, int sortNo)
        {
            Id = id;
            HpId = hpId;
            CenterCd = centerCd;
            CenterName = centerName;
            PrimaryKbn = primaryKbn;
            SortNo = sortNo;
        }

        public KensaCenterMstModel(long id, int hpId, string centerCd, string centerName, int primaryKbn, int sortNo, ModelStatus kensaCenterMstModelStatus)
        {
            Id = id;
            HpId = hpId;
            CenterCd = centerCd;
            CenterName = centerName;
            PrimaryKbn = primaryKbn;
            SortNo = sortNo;
            KensaCenterMstModelStatus = kensaCenterMstModelStatus;
        }

        public long Id { get; private set; }
        public int HpId { get; private set; }
        public string CenterCd { get; private set; }
        public string CenterName { get; private set; }
        public int PrimaryKbn { get; private set; }
        public int SortNo { get; private set; }
        public ModelStatus KensaCenterMstModelStatus { get; private set; }
    }
}
