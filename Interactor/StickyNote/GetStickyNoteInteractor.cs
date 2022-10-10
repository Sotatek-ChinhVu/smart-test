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

            var result = _ptTagRepository.SearchByPtId(inputData.HpId,inputData.PtId).ToList();
            if (result == null || !result.Any())
                return new GetStickyNoteOutputData(GetStickyNoteStatus.NoData);

            return new GetStickyNoteOutputData(result.ToList(), GetStickyNoteStatus.Successed);
        }
    }
}
