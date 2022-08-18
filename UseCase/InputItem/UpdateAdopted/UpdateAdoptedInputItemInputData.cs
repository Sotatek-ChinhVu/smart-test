using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.InputItem.UpdateAdopted
{
    public class UpdateAdoptedInputItemInputData : IInputData<UpdateAdoptedInputItemOutputData>
    {
        public UpdateAdoptedInputItemInputData(int valueAdopted, string itemCdInputItem, int sinDateInputItem)
        {
            ValueAdopted = valueAdopted;
            ItemCdInputItem = itemCdInputItem;
            SinDateInputItem = sinDateInputItem;
        }

        public int ValueAdopted { get; private set; }

        public string ItemCdInputItem { get; private set; }

        public int SinDateInputItem { get; private set; }
    }
}
