using Domain.CommonObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.Reception.Get
{
    public class GetReceptionInputData : IInputData<GetReceptionOutputData>
    {
        public RaiinNo RaiinNo { get; private set; }

        public GetReceptionInputData(long raiinNo)
        {
            RaiinNo = RaiinNo.From(raiinNo);
        }
    }
}
