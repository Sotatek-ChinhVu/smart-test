using Domain.Models.DrugInfor;
using UseCase.Core.Sync.Core;

namespace UseCase.DrugInfor.Get
{
    public class GetDrugInforOutputData : IOutputData
    {
        public GetDrugInforOutputData(DrugInforModel drugInfor, GetDrugInforStatus status)
        {
            DrugInfor = drugInfor;
            Status = status;
        }

        public DrugInforModel DrugInfor { get; private set; }

        public GetDrugInforStatus Status { get; private set; }

    }
}
