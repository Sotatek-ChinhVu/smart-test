using Domain.Models.PtTag;
using UseCase.StickyNote;

namespace Interactor.StickyNote
{
    public class GetStickyNoteInteractor : IGetStickyNoteInputPort
    {
        private readonly IPtTagRepository _ptTagRepository;

        public GetStickyNoteInteractor(IPtTagRepository ptTagRepository)
        {
            _ptTagRepository = ptTagRepository;
        }

        public GetStickyNoteOutputData Handle(GetStickyNoteInputData inputData)
        {
            if(inputData.HpId <= 0)
                return new GetStickyNoteOutputData(GetStickyNoteStatus.InvalidHpId);

            if (inputData.IsDeleted < 0)
                return new GetStickyNoteOutputData(GetStickyNoteStatus.InvalidIsDeleted);

            var result = _ptTagRepository.SearchByPtId(inputData.HpId,inputData.PtId,inputData.IsDeleted).ToList();
            if (result == null || !result.Any())
                return new GetStickyNoteOutputData(GetStickyNoteStatus.NoData);

            return new GetStickyNoteOutputData(result.ToList(), GetStickyNoteStatus.Successed);
        }
    }
}
