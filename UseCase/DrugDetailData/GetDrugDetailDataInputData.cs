using Domain.Models.DrugDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.DrugDetailData
{
    public class GetDrugDetailDataInputData: IInputData<GetDrugDetailDataOutputData>
    {
        public GetDrugDetailDataInputData(int selectedIndexOfChildren, int selectedIndexOfLevel0, string drugName, string itemCd, string yJCode)
        {
            SelectedIndexOfChildren = selectedIndexOfChildren;
            SelectedIndexOfLevel0 = selectedIndexOfLevel0;
            DrugName = drugName;
            ItemCd = itemCd;
            YJCode = yJCode;
        }

        public int SelectedIndexOfChildren { get; private set; }

        public int SelectedIndexOfLevel0 { get; private set; }

        public string DrugName { get; private set; }

        public string ItemCd { get; private set; }

        public string YJCode { get; private set; }
    }
}
