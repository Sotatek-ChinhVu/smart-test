using Entity.Tenant;
using Reporting.Statistics.Sta2010.Models;

namespace Reporting.Statistics.Sta2011.Models
{
    public class CoZaiReceInfModel : CoReceInfModel
    {
        public CoZaiReceInfModel(ReceInf receInf, PtHokenInf ptHokenInf,
            PtKohi ptKohi1, PtKohi ptKohi2, PtKohi ptKohi3, PtKohi ptKohi4,
            bool mainHokensyaNo, int prefNo, KaMst kaMst, UserMst tantoMst, bool isZaitaku) :
                base(receInf, ptHokenInf, ptKohi1, ptKohi2, ptKohi3, ptKohi4,
                    mainHokensyaNo, prefNo, kaMst, tantoMst)
        {
            IsZaitaku = isZaitaku;
        }

        public bool IsZaitaku { get; private set; }
    }
}
