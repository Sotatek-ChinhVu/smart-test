using UseCase.Core.Sync.Core;

namespace UseCase.OrdInfs.Validation
{
    public class ValidationOrdInfListInputData : IInputData<ValidationOrdInfListOutputData>
    {
        public ValidationOrdInfListInputData(List<ValidationOdrInfItem> odrInfs)
        {
            OdrInfs = odrInfs;
        }

        public List<ValidationOdrInfItem> OdrInfs { get; private set; }

        public List<ValidationOdrInfItem> ToList()
        {
            return OdrInfs;
        }
    }
}
