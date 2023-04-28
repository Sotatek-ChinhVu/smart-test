using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetSetDataTenMst
{
    public class GetSetDataTenMstInputData : IInputData<GetSetDataTenMstOutputData>
    {
        public GetSetDataTenMstInputData(int hpId, int sinDate, string itemCd, string jiCd, string ipnNameCd, string santeiItemCd, string agekasanCd1Note, string agekasanCd2Note, string agekasanCd3Note, string agekasanCd4Note)
        {
            HpId = hpId;
            SinDate = sinDate;
            ItemCd = itemCd;
            JiCd = jiCd;
            IpnNameCd = ipnNameCd;
            SanteiItemCd = santeiItemCd;
            AgekasanCd1Note = agekasanCd1Note;
            AgekasanCd2Note = agekasanCd2Note;
            AgekasanCd3Note = agekasanCd3Note;
            AgekasanCd4Note = agekasanCd4Note;
        }

        public int HpId { get; private set; }

        public int SinDate { get; private set; }

        #region SelectedTenMSt
        public string ItemCd { get; private set; }

        public string JiCd { get; private set; }

        public string IpnNameCd { get; private set; }

        public string SanteiItemCd { get; private set; }

        public string AgekasanCd1Note { get; private set; }

        public string AgekasanCd2Note { get; private set; }

        public string AgekasanCd3Note { get; private set; }

        public string AgekasanCd4Note { get; private set; }

        #endregion SelectedTenMst
    }
}
