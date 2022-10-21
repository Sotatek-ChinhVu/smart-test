using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.Reception.GetReceptionDefault
{
    public class GetReceptionDefaultInputData: IInputData<GetReceptionDefaultOutputData>
    {
        public GetReceptionDefaultInputData(int hpId, int ptId, int sindate, int defaultDoctorSetting)
        {
            HpId = hpId;
            PtId = ptId;
            Sindate = sindate;
            DefaultDoctorSetting = defaultDoctorSetting;
        }

        public int HpId { get; private set; }

        public int PtId { get; private set; }

        public int Sindate { get; private set; }

        public int DefaultDoctorSetting { get; private set; }
    }
}
