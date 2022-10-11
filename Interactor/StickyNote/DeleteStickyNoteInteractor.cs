using Domain.Models.PtTag;
using UseCase.StickyNote;

namespace Interactor.StickyNote
{
    public class DeleteStickyNoteInteractor : IDeleteStickyNoteInputPort
    {
        private readonly IPtTagRepository _ptTagRepository;

        public DeleteStickyNoteInteractor(IPtTagRepository ptTagRepository)
        {
            _ptTagRepository = ptTagRepository;
        }

        public DeleteStickyNoteOutputData Handle(DeleteStickyNoteInputData inputData)
        {
            if (inputData.HpId <= 0)
                return new DeleteStickyNoteOutputData(false, UpdateStickyNoteStatus.InvalidHpId);

            if (inputData.PtId <= 0)
                return new DeleteStickyNoteOutputData(false, UpdateStickyNoteStatus.InvalidPtId);

            if (inputData.SeqNo <= 0)
                return new DeleteStickyNoteOutputData(false, UpdateStickyNoteStatus.InvalidSeqNo);

            var result = _ptTagRepository.UpdateIsDeleted(inputData.HpId, inputData.PtId, inputData.SeqNo, 2);

            return new DeleteStickyNoteOutputData(result, result ? UpdateStickyNoteStatus.Successed : UpdateStickyNoteStatus.Failed);
        }
    }
}
