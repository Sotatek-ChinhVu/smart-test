using Domain.Models.MonshinInf;
using UseCase.Core.Sync.Core;

namespace UseCase.MonshinInfor.Save
{
    public class SaveMonshinInputData : IInputData<SaveMonshinOutputData>
    {
        public SaveMonshinInputData(List<MonshinInforModel> monshinInfors)
        {
            MonshinInfors = monshinInfors;
        }

        public List<MonshinInforModel> MonshinInfors { get; private set; }
    }
}