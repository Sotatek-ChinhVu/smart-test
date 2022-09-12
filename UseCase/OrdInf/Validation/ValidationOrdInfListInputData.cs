using UseCase.Core.Sync.Core;

namespace UseCase.OrdInfs.Validation
{
    public class ValidationOrdInfListInputData : IInputData<ValidationOrdInfListOutputData>
    {
        public ValidationOrdInfListInputData(List<ValidationOdrInfItem> ordInfs)
        {
            OrdInfs = ordInfs;
        }

        public List<ValidationOdrInfItem> OrdInfs { get; private set; }

        public List<ValidationOdrInfItem> ToList()
        {
            return OrdInfs;
        }
    }
}
