using Domain.Models.MstItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.CompareTenMst
{
    public class CompareTenMstOutputData: IOutputData
    {
        public CompareTenMstOutputData(List<CompareTenMstModel> listData, CompareTenMstStatus status)
        {
            ListData = listData;
            Status = status;
        }

        public List<CompareTenMstModel> ListData { get; private set; }
        public CompareTenMstStatus Status { get; private set; }
    }
}
