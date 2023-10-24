using Domain.Models.MstItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;
using UseCase.MstItem.GetCmtCheckMstList;

namespace UseCase.MstItem.GetAllCmtCheckMst
{
    public class GetAllCmtCheckMstOutputData : IOutputData
    {
        public GetAllCmtCheckMstOutputData(List<CommentCheckMstModel> itemCmtModels, GetCmtCheckMstListStatus status)
        {
            ItemCmtModels = itemCmtModels;
            Status = status;
        }

        public List<CommentCheckMstModel> ItemCmtModels { get; private set; }
        public GetCmtCheckMstListStatus Status { get; private set; }
    }
}
