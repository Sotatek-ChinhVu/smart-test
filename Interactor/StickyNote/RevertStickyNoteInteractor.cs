using Domain.Models.PtTag;
using UseCase.StickyNote;

namespace Interactor.StickyNote
{
    public class RevertStickyNoteInteractor : IRevertStickyNoteInputPort
    {
        private readonly IPtTagRepository _ptTagRepository;

        public RevertStickyNoteInteractor(IPtTagRepository ptTagRepository)
        {
            _ptTagRepository = ptTagRepository;
        }

        public RevertStickyNoteOutputData Handle(RevertStickyNoteInputData inputData)
        {
            if (inputData.HpId <= 0)
                return new RevertStickyNoteOutputData(false,RevertStickyNoteStatus.InvalidHpId);

            if (inputData.PtId <= 0)
                return new RevertStickyNoteOutputData(false,RevertStickyNoteStatus.InvalidPtId);

            if (inputData.SeqNo <= 0)
                return new RevertStickyNoteOutputData(false, RevertStickyNoteStatus.InvalidSeqNo);

            var result = _ptTagRepository.UpdateIsDeleted(inputData.HpId, inputData.PtId, inputData.SeqNo,0);

            return new RevertStickyNoteOutputData(result, result ? RevertStickyNoteStatus.Successed : RevertStickyNoteStatus.Failed);
        }
    }
}
