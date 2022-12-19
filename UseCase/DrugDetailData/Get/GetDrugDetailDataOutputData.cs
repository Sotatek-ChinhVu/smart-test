using Domain.Models.DrugDetail;
using UseCase.Core.Sync.Core;

namespace UseCase.DrugDetailData.Get
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
