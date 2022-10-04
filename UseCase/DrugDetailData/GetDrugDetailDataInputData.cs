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
        public GetDrugDetailDataInputData(int hpId, int sinDate, string itemCd, MenuInfModel drugMenu)
        {
            HpId = hpId;
            SinDate = sinDate;
            ItemCd = itemCd;
            DrugMenu = drugMenu;
        }

        public int HpId { get; private set; }

        public int SinDate { get; private set; }

        public string ItemCd { get; private set; }

        public MenuInfModel DrugMenu { get; private set; }
    }
}
