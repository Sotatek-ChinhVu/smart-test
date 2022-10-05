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
        public GetDrugDetailDataOutputData(DrugDetailModel data, GetDrugDetailDataStatus status)
        {
            Data = data;
            Status = status;
        }

        public DrugDetailModel Data { get; private set; }

        public GetDrugDetailDataStatus Status { get; private set; }

    }
}
