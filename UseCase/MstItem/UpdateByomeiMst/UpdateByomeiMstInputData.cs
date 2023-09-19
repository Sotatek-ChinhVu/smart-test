using Domain.Models.MstItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;
using UseCase.MstItem.UpdateAdoptedItemList;

namespace UseCase.MstItem.UpdateByomeiMst
{
    public class UpdateByomeiMstInputData: IInputData<UpdateByomeiMstOutputData>
    {
        public UpdateByomeiMstInputData(int hpId, int userId, List<UpdateByomeiMstModel> listData)
        {
            HpId = hpId;
            UserId = userId;
            ListData = listData;
        }

        public int HpId { get; private set; }
        public int UserId { get; private set; }
        public List<UpdateByomeiMstModel> ListData { get; private set; }
    }
}
