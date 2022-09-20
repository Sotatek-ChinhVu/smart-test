using Domain.Models.SpecialNote;
using UseCase.SpecialNote.Get;
using UseCase.SpecialNote.Save;

namespace Interactor.SpecialNote
{
    public class SaveSpecialNoteInteractor : ISaveSpecialNoteInputPort
    {
        private readonly ISpecialNoteRepository _specialNoteRepository;

        public SaveSpecialNoteInteractor(ISpecialNoteRepository specialNoteRepository)
        {
            _specialNoteRepository = specialNoteRepository;
        }

        public SaveSpecialNoteOutputData Handle(SaveSpecialNoteInputData inputData)
        {
            if (inputData.HpId <= 0)
            {
                return new SaveSpecialNoteOutputData(SaveSpecialNoteStatus.InvalidHpId);
            }
            if (inputData.PtId <= 0)
            {
                return new SaveSpecialNoteOutputData(SaveSpecialNoteStatus.InvalidPtId);
            }
            var result = _specialNoteRepository.SaveSpecialNote(inputData.HpId, inputData.PtId, inputData.SummaryTab, inputData.ImportantNoteTab, inputData.PatientInfoTab);
           
            if(!result) return new SaveSpecialNoteOutputData(SaveSpecialNoteStatus.Failed);
           
            return new SaveSpecialNoteOutputData(SaveSpecialNoteStatus.Successed);
        }
    }
}
