using Reporting.Statistics.Sta9000.Models;

namespace EmrCloudApi.Requests.PatientManagement
{
    public class SearchPtInfsRequest
    {
        public int OutputOrder { get; set; }

        public CoSta9000PtConf? CoSta9000PtConf { get; set; } = null;

        public CoSta9000HokenConf? CoSta9000HokenConf { get; set; } = null;

        public CoSta9000ByomeiConf? CoSta9000ByomeiConf { get; set; } = null;

        public CoSta9000RaiinConf? CoSta9000RaiinConf { get; set; } = null;

        public CoSta9000SinConf? CoSta9000SinConf { get; set; } = null;

        public CoSta9000KarteConf? CoSta9000KarteConf { get; set; } = null;

        public CoSta9000KensaConf? CoSta9000KensaConf { get; set; } = null;
    }
}
