using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.KohiHokenMst.Get
{
    public class GetKohiHokenMstInputData : IInputData<GetKohiHokenMstOutputData>
    {
        public int HpId { get; private set; }

        public int SinDate { get; private set; }

        public string FutansyaNo { get; private set; }

        public GetKohiHokenMstInputData(int hpId, int sinDate, string futansyaNo)
        {
            HpId = hpId;
            SinDate = sinDate;
            FutansyaNo = futansyaNo;
        }
    }
}
