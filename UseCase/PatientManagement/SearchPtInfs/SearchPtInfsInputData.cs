using Reporting.Statistics.Sta9000.Models;
using UseCase.Core.Sync.Core;

namespace UseCase.PatientManagement.SearchPtInfs
{
    public class SearchPtInfsInputData : IInputData<SearchPtInfsOutputData>
    {
        public SearchPtInfsInputData(int hpId, int outputOrder, CoSta9000PtConf coSta9000PtConf, CoSta9000HokenConf coSta9000HokenConf, CoSta9000ByomeiConf coSta9000ByomeiConf, CoSta9000RaiinConf coSta9000RaiinConf, CoSta9000SinConf coSta9000SinConf, CoSta9000KarteConf coSta9000KarteConf, CoSta9000KensaConf coSta9000KensaConf)
        {
            HpId = hpId;
            OutputOrder = outputOrder;
            CoSta9000PtConf = coSta9000PtConf;
            CoSta9000HokenConf = coSta9000HokenConf;
            CoSta9000ByomeiConf = coSta9000ByomeiConf;
            CoSta9000RaiinConf = coSta9000RaiinConf;
            CoSta9000SinConf = coSta9000SinConf;
            CoSta9000KarteConf = coSta9000KarteConf;
            CoSta9000KensaConf = coSta9000KensaConf;
        }

        public int HpId { get; private set; }

        public int OutputOrder { get; private set; }

        public CoSta9000PtConf CoSta9000PtConf { get; private set; }

        public CoSta9000HokenConf CoSta9000HokenConf { get; private set; }

        public CoSta9000ByomeiConf CoSta9000ByomeiConf { get; private set; }

        public CoSta9000RaiinConf CoSta9000RaiinConf { get; private set; }

        public CoSta9000SinConf CoSta9000SinConf { get; private set; }

        public CoSta9000KarteConf CoSta9000KarteConf { get; private set; }

        public CoSta9000KensaConf CoSta9000KensaConf { get; private set; }
    }
}
