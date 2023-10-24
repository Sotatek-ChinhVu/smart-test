using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetListUser
{
    public sealed class GetListUserInputData : IInputData<GetListUserOutputData>
    {
        public GetListUserInputData(int hpId, int userId, int sinDate)
        {
            HpId = hpId;
            UserId = userId;
            SinDate = sinDate;
        }
        public int HpId {  get; private set; }
        public int UserId {  get; private set; }
        public int SinDate {  get; private set; }
    }
}
