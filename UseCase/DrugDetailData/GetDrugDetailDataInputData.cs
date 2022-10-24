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
        public GetDrugDetailDataInputData(int selectedIndexOfMenuLelel, int level, string drugName, string itemCd, string yJCode)
        {
            SelectedIndexOfMenuLevel = selectedIndexOfMenuLelel;
            Level = level;
            DrugName = drugName;
            ItemCd = itemCd;
            YJCode = yJCode;
        }

        public int SelectedIndexOfMenuLevel { get; private set; }

        public int Level { get; private set; }

        public string DrugName { get; private set; }

        public string ItemCd { get; private set; }

        public string YJCode { get; private set; }
    }
}
