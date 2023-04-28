using UseCase.Core.Sync.Core;

namespace UseCase.OrdInfs.CheckOrdInfInDrug
{
    public class CheckOrdInfInDrugOutputData : IOutputData
    {
        public CheckOrdInfInDrugOutputData(bool result, CheckOrdInfInDrugStatus status)
        {
            Result = result;
            Status = status;
        }

        public bool Result { get; private set; }

        public CheckOrdInfInDrugStatus Status { get; private set; }
    }
}
