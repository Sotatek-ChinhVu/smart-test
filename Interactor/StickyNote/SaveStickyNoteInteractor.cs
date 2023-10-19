using Domain.Models.PtTag;
using UseCase.StickyNote;

namespace Interactor.StickyNote
{
    public class SaveStickyNoteInteractor : ISaveStickyNoteInputPort
    {
        private readonly IPtTagRepository _ptTagRepository;

        public SaveStickyNoteInteractor(IPtTagRepository ptTagRepository)
        {
            _ptTagRepository = ptTagRepository;
        }

        public SaveStickyNoteOutputData Handle(SaveStickyNoteInputData inputData)
        {
            try
            {
                var validate = Validate(inputData);
                if (validate != UpdateStickyNoteStatus.Successed)
                    return new SaveStickyNoteOutputData(false, validate);

                var result = _ptTagRepository.SaveStickyNote(inputData.stickyNoteModels, inputData.UserId);
                return new SaveStickyNoteOutputData(result, result ? UpdateStickyNoteStatus.Successed : UpdateStickyNoteStatus.Failed);
            }
            finally
            {
                _ptTagRepository.ReleaseResource();
            }
        }

        public UpdateStickyNoteStatus Validate(SaveStickyNoteInputData inputData)
        {
            if (inputData == null || inputData.stickyNoteModels == null || !inputData.stickyNoteModels.Any())
                return UpdateStickyNoteStatus.Failed;

            for(int i=0;i< inputData.stickyNoteModels.Count;i++)
            {
                if (inputData.stickyNoteModels[i].StartDate < 0 || inputData.stickyNoteModels[i].EndDate < 0 || inputData.stickyNoteModels[i].StartDate > inputData.stickyNoteModels[i].EndDate)
                {
                    return UpdateStickyNoteStatus.InvalidDate;
                }

                if (string.IsNullOrEmpty(inputData.stickyNoteModels[i].BackgroundColor))
                {
                    return UpdateStickyNoteStatus.InvalidColor;
                }

                if (inputData.stickyNoteModels[i].SeqNo == 0 && string.IsNullOrEmpty(inputData.stickyNoteModels[i].Memo))
                {
                    return UpdateStickyNoteStatus.InvalidMemo;
                }
                else if (string.IsNullOrEmpty(inputData.stickyNoteModels[i].Memo))
                {
                    var ptTag = _ptTagRepository.GetStickyNoteModel(inputData.stickyNoteModels[i].HpId, inputData.stickyNoteModels[i].PtId, inputData.stickyNoteModels[i].SeqNo);
                    if (ptTag.HpId == 0 || ptTag.PtId == 0 || ptTag.SeqNo == 0)
                    {
                        return UpdateStickyNoteStatus.InvalidSeqNo;
                    }
                    inputData.stickyNoteModels[i] = new StickyNoteModel(inputData.stickyNoteModels[i].HpId, inputData.stickyNoteModels[i].PtId, inputData.stickyNoteModels[i].SeqNo, ptTag.Memo, inputData.stickyNoteModels[i].StartDate, inputData.stickyNoteModels[i].EndDate, inputData.stickyNoteModels[i].IsDspUketuke, inputData.stickyNoteModels[i].IsDspKarte, inputData.stickyNoteModels[i].IsDspKaikei, inputData.stickyNoteModels[i].IsDspRece, inputData.stickyNoteModels[i].BackgroundColor, inputData.stickyNoteModels[i].TagGrpCd, inputData.stickyNoteModels[i].AlphablendVal, inputData.stickyNoteModels[i].FontSize, 1, inputData.stickyNoteModels[i].Width, inputData.stickyNoteModels[i].Height, inputData.stickyNoteModels[i].Left, inputData.stickyNoteModels[i].Top);
                }

                if (inputData.stickyNoteModels[i].SeqNo == 0 && inputData.stickyNoteModels[i].IsDeleted != 0)
                {
                    return UpdateStickyNoteStatus.InvalidMemo;
                }

                if (inputData.stickyNoteModels[i].HpId < 0) return UpdateStickyNoteStatus.InvalidHpId;
                if (inputData.stickyNoteModels[i].PtId < 0) return UpdateStickyNoteStatus.InvalidPtId;
                if (inputData.stickyNoteModels[i].SeqNo < 0) return UpdateStickyNoteStatus.InvalidSeqNo;
                if (inputData.stickyNoteModels[i].TagGrpCd < 0
                    || inputData.stickyNoteModels[i].IsDspKaikei < 0
                    || inputData.stickyNoteModels[i].IsDspKaikei > 1
                    || inputData.stickyNoteModels[i].IsDspKarte < 0
                    || inputData.stickyNoteModels[i].IsDspKarte > 1
                    || inputData.stickyNoteModels[i].IsDspRece < 0
                    || inputData.stickyNoteModels[i].IsDspRece > 1
                    || inputData.stickyNoteModels[i].AlphablendVal < 0
                    || inputData.stickyNoteModels[i].FontSize < 0
                    || inputData.stickyNoteModels[i].FontSize > 72
                    || inputData.stickyNoteModels[i].Height < 0
                    || inputData.stickyNoteModels[i].Left < 0
                    || inputData.stickyNoteModels[i].Top < 0
                    || inputData.stickyNoteModels[i].Width < 0)
                    return UpdateStickyNoteStatus.InvalidValue;
            }

            return UpdateStickyNoteStatus.Successed;
        }
    }
}
