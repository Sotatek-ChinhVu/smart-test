using CommonChecker.Models.OrdInfDetailModel;
using CommonChecker.Types;

namespace CommonChecker.Models.OrdInf
{
    public class OrdInfoModel : IOdrInfoModel<OrdInfoDetailModel>
    {
        public OrdInfoModel(int odrKouiKbn, int santeiKbn, List<OrdInfoDetailModel> ordInfDetails)
        {
            OdrKouiKbn = odrKouiKbn;
            SanteiKbn = santeiKbn;
            OrdInfDetails = ordInfDetails;
        }

        public int OdrKouiKbn { get; set; }
        public int SanteiKbn { get; set; }

        public List<OrdInfoDetailModel> OrdInfDetails { get; set; }

        // 処方 - Drug
        public bool IsDrug
        {
            get
            {
                return OdrKouiKbn >= 21 && OdrKouiKbn <= 23;
            }
        }

        // 注射 - Injection
        public bool IsInjection
        {
            get
            {
                return OdrKouiKbn >= 30 && OdrKouiKbn <= 34;
            }
        }

        public List<OrdInfoDetailModel> OdrInfDetailModelsIgnoreEmpty
        {
            get
            {
                if (OrdInfDetails == null)
                {
                    return new List<OrdInfoDetailModel>();
                }
                return new List<OrdInfoDetailModel>(OrdInfDetails.Where(o => !o.IsEmpty).ToList());
            }
        }

        public OrdInfoModel ChangeOdrDetail(List<OrdInfoDetailModel> ordInfDetails)
        {
            OrdInfDetails = ordInfDetails;
            return this;
        }
    }
}
