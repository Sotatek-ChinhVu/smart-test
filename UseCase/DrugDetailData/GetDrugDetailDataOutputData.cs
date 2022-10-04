using Domain.Models.DrugDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.DrugDetailData
{
    public class GetDrugDetailDataOutputData : IOutputData
    {
        public GetDrugDetailDataOutputData(List<DrugMenuItemModel> drugMenu, GetDrugDetailDataStatus status)
        {
            DrugMenu = drugMenu;
            Status = status;
        }

        public List<DrugMenuItemModel> DrugMenu { get; private set; }

        public GetDrugDetailDataStatus Status { get; private set; }

    }
}
