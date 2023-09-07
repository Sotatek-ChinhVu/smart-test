using Domain.Models.MstItem;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.UpdateSingleDoseMst
{
    public sealed class UpdateSingleDoseMstInputData : IInputData<UpdateSingleDoseMstOutputData>
    {
        public UpdateSingleDoseMstInputData(List<SingleDoseMstModel> singleDoseMsts)
        {
            SingleDoseMsts = singleDoseMsts;
        }
        public List<SingleDoseMstModel> SingleDoseMsts { get; set; }
    }
}
