using Domain.Models.Diseases;
using Domain.Models.MstItem;

namespace Domain.Models.TodayOdr
{
    public class CheckedDiseaseModel
    {
        public CheckedDiseaseModel(int sikkanCd, int nanByoCd, string byomei, string itemCd, int odrItemNo, string odrItemName, PtDiseaseModel ptDiseaseModel, ByomeiMstModel byomeiMst)
        {
            SikkanCd = sikkanCd;
            NanByoCd = nanByoCd;
            Byomei = byomei;
            ItemCd = itemCd;
            PtDiseaseModel = ptDiseaseModel;
            ByomeiMst = byomeiMst;
            OdrItemNo = odrItemNo;
            OdrItemName = odrItemName;
        }

        public int SikkanCd { get; private set; }

        public int NanByoCd { get; private set; }

        public string Byomei { get; private set; }

        public string ItemCd { get; private set; }

        public int OdrItemNo { get; private set; }

        public string OdrItemName { get; private set; }

        public PtDiseaseModel PtDiseaseModel { get; private set; }

        public ByomeiMstModel ByomeiMst { get; private set; }

        public bool IsAdopted
        {
            get => ByomeiMst?.IsAdopted == true;
        }
    }
}
