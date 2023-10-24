using Domain.Models.MstItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.SaveSetNameMnt
{
    public class SaveSetNameMntInputData: IInputData<SaveSetNameMntOutputData>
    {
        public SaveSetNameMntInputData(List<SetNameMntModel> listData, int hpId, int userId, int sindate)
        {
            ListData = listData;
            HpId = hpId;
            UserId = userId;
            Sindate = sindate;
        }

        public List<SetNameMntModel> ListData { get; private set; }
        public int HpId { get; private set; }
        public int UserId { get; private set; }
        public int Sindate { get; private set; }
    }
}
