using Entity.Tenant;

namespace Reporting.Sokatu.Common.Models
{
    public class CoKaMstModel
    {
        public KaMst KaMst { get; private set; }

        public CoKaMstModel(KaMst kaMst)
        {
            KaMst = kaMst;
        }

        public string? KaName
        {
            get => KaMst.KaName;
        }
    }
}
