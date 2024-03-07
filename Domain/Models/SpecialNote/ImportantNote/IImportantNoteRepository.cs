using Domain.Common;

namespace Domain.Models.SpecialNote.ImportantNote
{
    public interface IImportantNoteRepository : IRepositoryBase
    {
        List<PtAlrgyDrugModel> GetAlrgyDrugList(int hpId, long ptId);

        List<PtAlrgyDrugModel> GetAlrgyDrugList(long ptId, int sinDate);

        List<PtAlrgyElseModel> GetAlrgyElseList(int hpId, long ptId);

        List<PtAlrgyElseModel> GetAlrgyElseList(long ptId, int sinDate);

        List<PtAlrgyFoodModel> GetAlrgyFoodList(int hpId, long ptId);

        List<PtAlrgyFoodModel> GetAlrgyFoodList(int hpId, long ptId, int sinDate);

        List<PtOtcDrugModel> GetOtcDrugList(int hpId, long ptId);

        List<PtOtcDrugModel> GetOtcDrugList(long ptId, int sinDate);

        List<PtOtherDrugModel> GetOtherDrugList(int hpId, long ptId);

        List<PtOtherDrugModel> GetOtherDrugList(long ptId, int sinDate);

        List<PtSuppleModel> GetSuppleList(int hpId, long ptId);

        List<PtSuppleModel> GetSuppleList(long ptId, int sinDate);

        List<PtInfectionModel> GetInfectionList(int hpId, long ptId);

        List<PtKioRekiModel> GetKioRekiList(int hpId, long ptId);

        void AddAlrgyDrugList(List<PtAlrgyDrugModel> inputDatas, int hpId, int userId);
    }
}
