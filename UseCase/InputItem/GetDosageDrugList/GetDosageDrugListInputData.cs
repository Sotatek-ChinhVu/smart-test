using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.InputItem.GetDosageDrugList
{
    public class GetDosageDrugListInputData : IInputData<GetDosageDrugListOutputData>
    {
        public GetDosageDrugListInputData(List<string> yjCds)
        {
            YjCds = yjCds;
        }

        public List<string> YjCds { get; private set; }

        public List<string> ToList()
        {
            return YjCds;
        }
    }
}
