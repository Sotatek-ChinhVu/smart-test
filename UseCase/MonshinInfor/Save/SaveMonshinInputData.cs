using Domain.Models.MonshinInf;
using UseCase.Core.Sync.Core;

namespace UseCase.MonshinInfor.Save
{
    public class SaveMonshinInputData : IInputData<SaveMonshinOutputData>
    {
        public SaveMonshinInputData(List<MonshinInforModel> monshinInfors, int userId)
        {
            MonshinInfors = monshinInfors;
            UserId = userId;
        }

        public List<MonshinInforModel> MonshinInfors { get; private set; }

        public int UserId { get; private set; }
    }
}