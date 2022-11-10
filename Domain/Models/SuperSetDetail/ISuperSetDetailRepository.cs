namespace Domain.Models.SuperSetDetail;

public interface ISuperSetDetailRepository
{
    SuperSetDetailModel GetSuperSetDetail(int hpId, int setCd, int sindate);

    int SaveSuperSetDetail(int setCd, int userId, int hpId, List<SetByomeiModel> SetByomeiList, SetKarteInfModel SetKarteInf, List<SetOrderInfModel> ListSetOrdInfModels);

    bool SaveListSetKarteImgTemp(List<SetKarteImgInfModel> listModel);

    List<SetOrderInfModel> GetOnlyListOrderInfModel(int hpId, int setCd);

    (List<SetByomeiModel> byomeis, List<SetKarteInfModel> karteInfs, List<SetOrderInfModel>) GetSuperSetDetailForTodayOrder(int hpId, int setCd);
}