using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.UpdateAdopted
{
    public class UpdateAdoptedTenItemInputData : IInputData<UpdateAdoptedTenItemOutputData>
    {
        public UpdateAdoptedTenItemInputData(int valueAdopted, string itemCdInputItem, int startDateInputItem)
        {
            ValueAdopted = valueAdopted;
            ItemCdInputItem = itemCdInputItem;
            StartDateInputItem = startDateInputItem;
        }

        public int ValueAdopted { get; private set; }

        public string ItemCdInputItem { get; private set; }

        public int StartDateInputItem { get; private set; }
    }
}
