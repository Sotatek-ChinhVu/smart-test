using Domain.Models.InputItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.InputItem.GetDosageDrugList
{
    public class GetDosageDrugListOutputData: IOutputData
    {
        public GetDosageDrugListOutputData(List<DosageDrugModel> listDosageDrugModel, GetDosageDrugListStatus status)
        {
            ListDosageDrugModel = listDosageDrugModel;
            Status = status;
        }

        public List<DosageDrugModel> ListDosageDrugModel { get; private set; }

        public GetDosageDrugListStatus Status { get; private set; }

    }
}
