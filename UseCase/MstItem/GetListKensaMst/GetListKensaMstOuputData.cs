using Domain.Models.KensaIrai;
using Domain.Models.MstItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;
using UseCase.MstItem.SearchPostCode;
using UseCase.MstItem.SearchTenMstItem;

namespace UseCase.MstItem.GetListResultKensaMst
{
    public class GetListKensaMstOuputData : IOutputData
    {
        public GetListKensaMstOuputData(List<KensaMstModel> kensaMsts, SearchTenMstItemStatus status)
        {
            KensaMsts = kensaMsts;
            Status = status;
        }

        public List<KensaMstModel> KensaMsts { get; private set; }
        public SearchTenMstItemStatus Status { get; private set; }
    }
}
