using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Async.Core;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.IsKensaItemOrdering
{
    public class IsKensaItemOrderingInputData : IInputData<IsKensaItemOrderingOutputData>
    {
        public IsKensaItemOrderingInputData(int hpId, string tenItemCd) 
        {
            HpId = hpId;
            TenItemCd = tenItemCd;
        }

        public int HpId { get; private set; }

        public string TenItemCd { get; private set; }
    }
}
