using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetListSetGenerationMst
{
    public sealed class GetListSetGenerationMstInputData : IInputData<GetListSetGenerationMstOutputData>
    {
        public GetListSetGenerationMstInputData(int hpId)
        {
            HpId = hpId;
        }

        public int HpId { get; private set; }
    }
}
