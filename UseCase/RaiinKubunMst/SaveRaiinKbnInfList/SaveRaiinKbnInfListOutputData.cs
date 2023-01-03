using UseCase.Core.Sync.Core;

namespace UseCase.RaiinKubunMst.SaveRaiinKbnInfList
{
    public class SaveRaiinKbnInfListOutputData : IOutputData
    {
        public SaveRaiinKbnInfListOutputData(SaveRaiinKbnInfListStatus status)
        {
            Status = status;
        }

        public SaveRaiinKbnInfListStatus Status { get; private set; }
    }
}
