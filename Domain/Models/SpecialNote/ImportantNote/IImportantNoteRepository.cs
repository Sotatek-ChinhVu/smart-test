namespace Domain.Models.SpecialNote.ImportantNote
{
    public interface IImportantNoteRepository
    {
        List<PtAlrgyDrugModel> GetAlrgyDrugList(long ptId);

        List<PtAlrgyElseModel> GetAlrgyElseList(long ptId);

        List<PtAlrgyFoodModel> GetAlrgyFoodList(long ptId);

        List<PtOtcDrugModel> GetOtcDrugList(long ptId);

        List<PtOtherDrugModel> GetOtherDrugList(long ptId);

        List<PtSuppleModel> GetSuppleList(long ptId);

        List<PtInfectionModel> GetInfectionList(long ptId);

        List<PtKioRekiModel> GetKioRekiList(long ptId);

        void AddAlrgyDrugList(List<PtAlrgyDrugModel> inputDatas);
    }
}
