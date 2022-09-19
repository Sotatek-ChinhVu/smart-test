﻿namespace Domain.Models.SuperSetDetail;

public interface ISuperSetDetailRepository
{
    SuperSetDetailModel GetSuperSetDetail(int hpId, int setCd, int sindate);

    int SaveSuperSetDetail(int setCd, int userId, int hpId, SuperSetDetailModel superSetDetailModel);

    bool SaveListSetKarteImgTemp(List<SetKarteImgInfModel> listModel);
}