using Domain.Models.SpecialNote;
using Domain.Models.SpecialNote.PatientInfo;
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
            try
            {
                if (inputData.HpId <= 0)
                {
                    return new SaveSpecialNoteOutputData(SaveSpecialNoteStatus.InvalidHpId);
                }
                if (inputData.PtId <= 0)
                {
                    return new SaveSpecialNoteOutputData(SaveSpecialNoteStatus.InvalidPtId);
                }
                var result = _specialNoteRepository.SaveSpecialNote(inputData.HpId, inputData.PtId, inputData.SummaryTab, inputData.ImportantNoteTab, new PatientInfoModel(inputData.PatientInfoTab.PregnancyItems, inputData.PatientInfoTab.PtCmtInfItems, inputData.PatientInfoTab.SeikatureInfItems, new List<PhysicalInfoModel> { new PhysicalInfoModel(inputData.PatientInfoTab.KensaInfDetailModels) }), inputData.UserId);

                if (!result) return new SaveSpecialNoteOutputData(SaveSpecialNoteStatus.Failed);

                return new SaveSpecialNoteOutputData(SaveSpecialNoteStatus.Successed);
            }
            finally
            {
                _specialNoteRepository.ReleaseResource();
            }
        }
    }
}
