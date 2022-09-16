using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.MonshinInfor.GetList
{
    public class GetMonshinInforListInputData:IInputData<GetMonshinInforListOutputData>
    {
        public GetMonshinInforListInputData(int hpId, long ptId)
        {
            HpId = hpId;
            PtId = ptId;
        }

        public int HpId { get; set; }
        public long PtId { get; set; }
    }
}
