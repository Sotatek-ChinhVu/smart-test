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
        public GetTenMstByCodeInputData(string itemCd, int setKbn, int sinDate)
        {
            ItemCd = itemCd;
            SetKbn = setKbn;
            SinDate = sinDate;
        }

        public string ItemCd {  get; private set; } 
        public int SetKbn {  get; private set; } 
        public int SinDate {  get; private set; } 
    }
}
