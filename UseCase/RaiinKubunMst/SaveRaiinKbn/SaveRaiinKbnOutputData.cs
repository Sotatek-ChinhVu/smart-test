using Domain.Models.Reception;
using UseCase.Core.Sync.Core;

namespace UseCase.RaiinKubunMst.SaveRaiinKbn
{
    public class SaveRaiinKbnOutputData : IOutputData
    {
        public ReceptionModel ReceptionModel { get; private set; }

        public SaveRaiinKbnStatus Status { get; private set; }

        public SaveRaiinKbnOutputData(ReceptionModel receptionModel, SaveRaiinKbnStatus status)
        {
            ReceptionModel = receptionModel;
            Status = status;
        }
    }
}
