using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetDosageDrugList
{
    public class GetDosageDrugListInputData : IInputData<GetDosageDrugListOutputData>
    {
        public GetDosageDrugListInputData(int hpId, List<string> yjCds)
        {
            YjCds = yjCds;
            HpId = hpId;
        }

        public List<string> YjCds { get; private set; }

        public int HpId { get; private set; }

        public List<string> ToList()
        {
            return YjCds;
        }
    }
}
