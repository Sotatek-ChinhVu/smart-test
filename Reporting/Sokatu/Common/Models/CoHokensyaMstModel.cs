using Entity.Tenant;

namespace Reporting.Sokatu.Common.Models
{
    public class CoHokensyaMstModel
    {
        public HokensyaMst HokensyaMst { get; private set; }

        public CoHokensyaMstModel(HokensyaMst hokensyaMst)
        {
            HokensyaMst = hokensyaMst;
        }

        public string HokensyaNo
        {
            get => HokensyaMst.HokensyaNo ?? string.Empty;
        }

        public string Name
        {
            get => HokensyaMst.Name ?? string.Empty;
        }
    }
}
