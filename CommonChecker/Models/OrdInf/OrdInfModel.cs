

using CommonChecker.Types;

namespace CommonChecker.Models.OrdInf
{
    public class OrdInfModel : IOdrInfModel<OrdInfDetailModel>
    {
        public int HpId { get; private set; }
        public long RaiinNo { get; private set; }
        public long RpNo { get; private set; }
        public long RpEdaNo { get; private set; }
        public long PtId { get; set; }
        public int SinDate { get; private set; }
        public int HokenPid { get; private set; }
        public int OdrKouiKbn { get; private set; }
        public string RpName { get; private set; }
        public int InoutKbn { get; private set; }
        public int SikyuKbn { get; private set; }
        public int SyohoSbt { get; private set; }
        public int SanteiKbn { get; private set; }
        public int TosekiKbn { get; private set; }
        public int DaysCnt { get; private set; }
        public int SortNo { get; private set; }
        public int IsDeleted { get; private set; }
        public long Id { get; private set; }
        public DateTime CreateDate { get; private set; }
        public int CreateId { get; private set; }
        public string CreateName { get; private set; }

        public GroupKoui GroupKoui { get; private set; }
        public List<OrdInfDetailModel> OrdInfDetails { get; private set; }
        public DateTime UpdateDate { get; private set; }

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

        public List<OrdInfDetailModel> OdrInfDetailModelsIgnoreEmpty
        {
            get
            {
                if (OrdInfDetails == null)
                {
                    return new List<OrdInfDetailModel>();
                }
                return new List<OrdInfDetailModel>(OrdInfDetails.Where(o => !o.IsEmpty).ToList());
            }
        }
    }
}
