using Domain.Models.Diseases;
using Domain.Models.MstItem;

namespace Domain.Models.TodayOdr
{
    public class CheckedDiseaseModel
    {
        public CheckedDiseaseModel(int sikkanCd, int nanByoCd, string byomei, int odrItemNo, PtDiseaseModel ptDiseaseModel, ByomeiMstModel byomeiMst)
        {
            SikkanCd = sikkanCd;
            NanByoCd = nanByoCd;
            Byomei = byomei;
            PtDiseaseModel = ptDiseaseModel;
            ByomeiMst = byomeiMst;
            OdrItemNo = odrItemNo;
        }

        public int SikkanCd { get; private set; }

        public int NanByoCd { get; private set; }

        public string Byomei { get; private set; }

        public int OdrItemNo { get; private set; }

        public PtDiseaseModel PtDiseaseModel { get; private set; }

        public ByomeiMstModel ByomeiMst { get; private set; }

        public bool IsAdopted
        {
            get => ByomeiMst?.IsAdopted == true;
        }
    }
}
