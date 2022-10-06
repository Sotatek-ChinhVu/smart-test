using UseCase.Core.Sync.Core;

namespace UseCase.OrdInfs.ValidationInputItem
{
    public class ValidationInputItemInputData : IInputData<ValidationOrdInfListOutputData>
    {
        public ValidationInputItemInputData(List<ValidationOdrInfItem> odrInfs)
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
