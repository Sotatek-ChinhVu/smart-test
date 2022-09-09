using Domain.Models.DrugDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.DrugDetail
{
    public class GetDrugDetailOutputData : IOutputData
    {
        public GetDrugDetailOutputData(List<DrugMenuItemModel> drugMenu, GetDrugDetailStatus status)
        {
            DrugMenu = drugMenu;
            Status = status;
        }

        public List<DrugMenuItemModel> DrugMenu { get; private set; }

        public GetDrugDetailStatus Status { get; private set; }

    }
}
