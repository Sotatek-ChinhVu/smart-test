using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetTenMstByCode
{
    public sealed class GetTenMstByCodeInputData : IInputData<GetTenMstByCodeOutputData>
    {
        public GetTenMstByCodeInputData(int hpId, string itemCd, int setKbn, int sinDate)
        {
            HpId = hpId;
            ItemCd = itemCd;
            SetKbn = setKbn;
            SinDate = sinDate;
        }

        public int HpId { get; private set; }
        public string ItemCd { get; private set; }
        public int SetKbn { get; private set; }
        public int SinDate { get; private set; }
    }
}
