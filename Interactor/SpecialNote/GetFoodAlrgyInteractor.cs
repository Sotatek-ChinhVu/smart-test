using Domain.Models.SpecialNote.ImportantNote;
using UseCase.SpecialNote.Get;

namespace Interactor.SpecialNote
{
    public class GetFoodAlrgyInteractor : IGetFoodAlrgyInputPort
    {
        private readonly IImportantNoteRepository _importantNoteRepository;

        public GetFoodAlrgyInteractor(IImportantNoteRepository importantNoteRepository)
        {
            _importantNoteRepository = importantNoteRepository;
        }

        public GetFoodAlrgyOutputData Handle(GetFoodAlrgyInputData inputData)
        {
            var foodAlrgyMasterData = GetFoodAlrgyMasterData();
            return new GetFoodAlrgyOutputData(foodAlrgyMasterData, GetFoodAlrgyStatus.Successed);
        }
        private FoodAlrgyMasterData GetFoodAlrgyMasterData()
        {
            return new FoodAlrgyMasterData(_importantNoteRepository.GetFoodAlrgyMasterData()); 
        }
    }
}
