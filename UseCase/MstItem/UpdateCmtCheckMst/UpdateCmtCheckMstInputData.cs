using Domain.Models.MstItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.UpdateCmtCheckMst
{
    public class UpdateCmtCheckMstInputData : IInputData<UpdateCmtCheckMstOutputData>
    {
        public int UserId { get; private set; }
        public int HpId { get; private set; }
        public List<ItemCmtModel> ListItemCmt { get; private set; }

       public UpdateCmtCheckMstInputData(int userId, int hpId, List<ItemCmtModel> listItemCmt)
        {
            UserId = userId;
            HpId = hpId;
            ListItemCmt = listItemCmt;
        }
    }
}
